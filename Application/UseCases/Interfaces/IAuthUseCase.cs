using BGarden.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BGarden.Application.UseCases.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с аутентификацией и авторизацией
    /// </summary>
    public interface IAuthUseCase
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerDto">Данные для регистрации</param>
        /// <returns>Токен доступа</returns>
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="loginDto">Данные для входа</param>
        /// <returns>Токен доступа или null если требуется 2FA</returns>
        Task<TokenDto?> LoginAsync(LoginDto loginDto);
        
        /// <summary>
        /// Обновление токена доступа
        /// </summary>
        /// <param name="token">Токен обновления</param>
        /// <param name="ipAddress">IP-адрес клиента</param>
        /// <returns>Новый токен доступа</returns>
        Task<TokenDto> RefreshTokenAsync(string token, string ipAddress);
        
        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        /// <param name="refreshToken">Токен обновления для отзыва</param>
        /// <returns>Успешность операции</returns>
        Task<bool> LogoutAsync(string refreshToken);
        
        /// <summary>
        /// Завершение авторизации с двухфакторной аутентификацией
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="twoFactorCode">Код двухфакторной аутентификации</param>
        /// <param name="rememberMe">Запомнить пользователя</param>
        /// <returns>Токен доступа</returns>
        Task<TokenDto> VerifyTwoFactorAsync(string username, string twoFactorCode, bool rememberMe = false);
        
        /// <summary>
        /// Настройка двухфакторной аутентификации
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>Данные для настройки</returns>
        Task<TwoFactorSetupDto> SetupTwoFactorAsync(string username);
        
        /// <summary>
        /// Включение двухфакторной аутентификации
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="verificationCode">Проверочный код</param>
        /// <returns>Успешность операции</returns>
        Task<bool> EnableTwoFactorAsync(string username, string verificationCode);
        
        /// <summary>
        /// Отключение двухфакторной аутентификации
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="verificationCode">Проверочный код</param>
        /// <returns>Успешность операции</returns>
        Task<bool> DisableTwoFactorAsync(string username, string verificationCode);
        
        /// <summary>
        /// Получение истории аутентификации пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>История аутентификации</returns>
        Task<IEnumerable<AuthLogDto>> GetAuthHistoryAsync(string username);
        
        /// <summary>
        /// Разблокировка заблокированного пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns>Успешность операции</returns>
        Task<bool> UnlockUserAsync(string username);
    }
} 