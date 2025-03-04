using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Application.DTO;
using BGarden.Application.Mappers;
using BGarden.Application.UseCases.Interfaces;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

namespace BGarden.Application.UseCases
{
    /// <summary>
    /// Сервис для работы с аутентификацией и авторизацией
    /// </summary>
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthUseCase> _logger;

        /// <summary>
        /// Конструктор сервиса авторизации
        /// </summary>
        public AuthUseCase(
            IAuthService authService, 
            IJwtService jwtService,
            ILogger<AuthUseCase> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Проверка сложности пароля
                if (!IsStrongPassword(registerDto.Password))
                {
                    throw new InvalidOperationException("Пароль должен содержать минимум 8 символов, включая заглавные и строчные буквы, цифры и специальные символы");
                }
                
                // Создаем нового пользователя используя маппер
                var user = registerDto.ToEntity();
                
                // Вызываем сервис создания пользователя
                user = await _authService.CreateUserAsync(user, registerDto.Password, registerDto.IpAddress);
                
                _logger.LogInformation("Успешная регистрация пользователя {Username}", registerDto.Username);
                
                // Генерируем JWT токен используя JwtService вместо AuthService
                var jwtToken = _jwtService.GenerateJwtToken(user);
                
                // Генерируем refresh токен
                var refreshToken = await _authService.GenerateRefreshTokenAsync(user, registerDto.IpAddress ?? "unknown");
                
                // Возвращаем токены
                return AuthMapper.CreateTokenDto(
                    jwtToken, 
                    refreshToken, 
                    DateTime.UtcNow.AddMinutes(15), // Время жизни токена
                    user.Username
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя {Username}: {Message}", 
                    registerDto.Username, ex.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TokenDto?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(
                    loginDto.Username, 
                    loginDto.Password, 
                    loginDto.IpAddress,
                    loginDto.UserAgent
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Неудачная попытка входа для пользователя {Username}", loginDto.Username);
                    return null;
                }
                
                // Если требуется 2FA и она включена
                if (user.TwoFactorEnabled && string.IsNullOrEmpty(loginDto.TwoFactorCode))
                {
                    _logger.LogInformation("Требуется двухфакторная аутентификация для пользователя {Username}", loginDto.Username);
                    return new TokenDto 
                    { 
                        Username = user.Username,
                        RequiresTwoFactor = true
                    };
                }
                
                // Если включена 2FA, проверяем код
                if (user.TwoFactorEnabled && !string.IsNullOrEmpty(loginDto.TwoFactorCode))
                {
                    bool isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, loginDto.TwoFactorCode);
                    if (!isValid)
                    {
                        _logger.LogWarning("Неверный код 2FA для пользователя {Username}", loginDto.Username);
                        throw new InvalidOperationException("Неверный код двухфакторной аутентификации");
                    }
                }
                
                _logger.LogInformation("Успешный вход пользователя {Username}", loginDto.Username);
                
                // Генерируем JWT токен используя JwtService
                var jwtToken = _jwtService.GenerateJwtToken(user);
                
                // Генерируем refresh токен
                var refreshToken = await _authService.GenerateRefreshTokenAsync(user, loginDto.IpAddress ?? "unknown");
                
                // Возвращаем токены
                return AuthMapper.CreateTokenDto(
                    jwtToken, 
                    refreshToken, 
                    DateTime.UtcNow.AddMinutes(15), // Время жизни токена
                    user.Username
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при авторизации пользователя {Username}: {Message}", 
                    loginDto.Username, ex.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TokenDto> RefreshTokenAsync(string token, string ipAddress)
        {
            try
            {
                var (user, refreshToken) = await _authService.RefreshTokenAsync(token, ipAddress);
                
                // Генерируем новый JWT токен
                var jwtToken = _jwtService.GenerateJwtToken(user);
                
                return AuthMapper.CreateTokenDto(
                    jwtToken,
                    refreshToken,
                    DateTime.UtcNow.AddMinutes(15), // Время жизни токена
                    user.Username
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении токена: {Message}", ex.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            try
            {
                await _authService.RevokeTokenAsync(refreshToken, "unknown");
                _logger.LogInformation("Пользователь вышел из системы");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выходе из системы");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<TokenDto> VerifyTwoFactorAsync(string username, string twoFactorCode, bool rememberMe = false)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(await GetUserIdFromUsername(username));
                
                if (user == null || !user.IsActive)
                {
                    throw new InvalidOperationException("Пользователь не найден или заблокирован");
                }
                
                if (!user.TwoFactorEnabled)
                {
                    throw new InvalidOperationException("Двухфакторная аутентификация не включена для данного пользователя");
                }
                
                // Проверяем код 2FA
                bool isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, twoFactorCode);
                if (!isValid)
                {
                    throw new InvalidOperationException("Неверный код двухфакторной аутентификации");
                }
                
                // Генерируем JWT токен используя JwtService
                var jwtToken = _jwtService.GenerateJwtToken(user);
                
                // Генерируем refresh токен
                var refreshToken = await _authService.GenerateRefreshTokenAsync(user, "2FA Verification");
                
                _logger.LogInformation("Пользователь {Username} прошел 2FA верификацию", username);
                
                // Срок действия по умолчанию 15 минут, увеличиваем если "запомнить меня"
                var expiryTime = rememberMe ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddMinutes(15);
                
                return AuthMapper.CreateTokenDto(
                    jwtToken, 
                    refreshToken, 
                    expiryTime,
                    username
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при верификации 2FA для пользователя {Username}: {Message}", 
                    username, ex.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TwoFactorSetupDto> SetupTwoFactorAsync(string username)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при настройке 2FA", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Настраиваем двухфакторную аутентификацию
                string qrCodeUrl = await _authService.SetupTwoFactorAsync(user.Id);
                
                _logger.LogInformation("Двухфакторная аутентификация успешно настроена для пользователя {Username}", username);
                
                // Генерируем секретный ключ из QR-кода
                string secretKey = Regex.Match(qrCodeUrl, "secret=([A-Z0-9]+)").Groups[1].Value;
                
                return AuthMapper.CreateTwoFactorSetupDto(secretKey, qrCodeUrl, user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при настройке двухфакторной аутентификации для пользователя {Username}", username);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> EnableTwoFactorAsync(string username, string verificationCode)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при включении 2FA", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Проверяем код подтверждения
                bool isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, verificationCode);
                if (!isValid)
                {
                    _logger.LogWarning("Неверный код подтверждения при включении 2FA для пользователя {Username}", username);
                    return false;
                }
                
                // Включаем двухфакторную аутентификацию в модели пользователя
                user.TwoFactorEnabled = true;
                await _authService.UpdateUserAsync(user);
                
                _logger.LogInformation("Двухфакторная аутентификация включена для пользователя {Username}", username);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при включении двухфакторной аутентификации для пользователя {Username}", username);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DisableTwoFactorAsync(string username, string verificationCode)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при отключении 2FA", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Проверяем код подтверждения
                bool isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, verificationCode);
                if (!isValid)
                {
                    _logger.LogWarning("Неверный код подтверждения при отключении 2FA для пользователя {Username}", username);
                    return false;
                }
                
                // Отключаем двухфакторную аутентификацию
                await _authService.DisableTwoFactorAsync(user.Id);
                
                _logger.LogInformation("Двухфакторная аутентификация отключена для пользователя {Username}", username);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отключении двухфакторной аутентификации для пользователя {Username}", username);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AuthLogDto>> GetAuthHistoryAsync(string username)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при запросе истории аутентификации", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Получаем журнал авторизаций для пользователя
                var authLogs = await _authService.GetUserAuthLogsAsync(user.Id);
                
                _logger.LogInformation("Получена история аутентификации для пользователя {Username}", username);
                
                return authLogs.ToDtos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении истории аутентификации для пользователя {Username}", username);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UnlockUserAsync(string username)
        {
            try
            {
                // Получаем пользователя по имени
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при разблокировке", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Разблокируем аккаунт пользователя
                await _authService.UnlockUserAccountAsync(user.Id);
                
                _logger.LogInformation("Аккаунт пользователя {Username} разблокирован", username);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при разблокировке аккаунта пользователя {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// Проверка сложности пароля
        /// </summary>
        private bool IsStrongPassword(string password)
        {
            var hasUpperCase = new Regex(@"[A-Z]");
            var hasLowerCase = new Regex(@"[a-z]");
            var hasDigit = new Regex(@"[0-9]");
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]");
            
            return password.Length >= 8 && 
                   hasUpperCase.IsMatch(password) && 
                   hasLowerCase.IsMatch(password) && 
                   hasDigit.IsMatch(password) && 
                   hasSpecialChar.IsMatch(password);
        }

        // Вспомогательный метод для получения ID пользователя по имени
        private async Task<int> GetUserIdFromUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Имя пользователя не может быть пустым", nameof(username));
                
            var user = await _authService.AuthenticateAsync(username, "dummy_password_not_used", null);
            return user?.Id ?? 0;
        }
    }
} 