using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Application.DTO;
using BGarden.Application.Mappers;
using BGarden.Application.UseCases.Interfaces;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace BGarden.Application.UseCases
{
    /// <summary>
    /// Сервис для работы с аутентификацией и авторизацией
    /// </summary>
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthUseCase> _logger;

        /// <summary>
        /// Конструктор сервиса авторизации
        /// </summary>
        public AuthUseCase(IAuthService authService, ILogger<AuthUseCase> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                
                // Если требуется 2FA и предоставлен код
                if (user.TwoFactorEnabled && !string.IsNullOrEmpty(loginDto.TwoFactorCode))
                {
                    var isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, loginDto.TwoFactorCode);
                    if (!isValid)
                    {
                        _logger.LogWarning("Неверный код 2FA для пользователя {Username}", loginDto.Username);
                        return null;
                    }
                }
                
                // Генерируем JWT токен
                var jwtToken = await _authService.GenerateJwtTokenAsync(user);
                var refreshToken = await _authService.GenerateRefreshTokenAsync(user, loginDto.IpAddress ?? "Unknown");
                
                // Определяем срок действия токена (1 час или 24 часа, если "запомнить меня")
                var expiration = DateTime.UtcNow.AddMinutes(loginDto.RememberMe ? 1440 : 60);
                
                _logger.LogInformation("Пользователь {Username} успешно вошел в систему", loginDto.Username);
                
                return AuthMapper.CreateTokenDto(jwtToken, refreshToken, expiration, user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при попытке входа пользователя {Username}", loginDto.Username);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var (jwtToken, refreshToken) = await _authService.RefreshTokenAsync(
                    refreshTokenDto.Token, 
                    refreshTokenDto.IpAddress ?? "unknown"
                );
                
                // Предполагаем, что токен действителен на 60 минут
                DateTime expiration = DateTime.UtcNow.AddMinutes(60);
                
                var user = await _authService.GetUserByIdAsync(refreshToken.UserId);
                
                _logger.LogInformation("Токен доступа обновлен для пользователя {Username}", user?.Username);
                
                return AuthMapper.CreateTokenDto(jwtToken, refreshToken, expiration, user?.Username ?? "unknown");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении токена");
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
                var user = await _authService.GetUserByIdAsync(
                    (await _authService.AuthenticateAsync(username, "dummy_password_not_used", null))?.Id ?? 0
                );
                
                if (user == null)
                {
                    _logger.LogWarning("Пользователь {Username} не найден при проверке 2FA", username);
                    throw new InvalidOperationException($"Пользователь {username} не найден");
                }
                
                // Проверяем код двухфакторной аутентификации
                bool isValid = await _authService.VerifyTwoFactorCodeAsync(user.Id, twoFactorCode);
                if (!isValid)
                {
                    _logger.LogWarning("Неверный код двухфакторной аутентификации для пользователя {Username}", username);
                    throw new InvalidOperationException("Неверный код двухфакторной аутентификации");
                }
                
                // Генерируем JWT токен
                string jwtToken = await _authService.GenerateJwtTokenAsync(user);
                
                // Генерируем Refresh токен
                var refreshToken = await _authService.GenerateRefreshTokenAsync(user, "unknown");
                
                // Определяем срок действия токена
                DateTime expiration = DateTime.UtcNow.AddMinutes(rememberMe ? 1440 : 60); // 24 часа или 1 час
                
                _logger.LogInformation("Двухфакторная аутентификация успешно пройдена для пользователя {Username}", username);
                
                return AuthMapper.CreateTokenDto(jwtToken, refreshToken, expiration, user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке двухфакторной аутентификации для пользователя {Username}", username);
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
    }
} 