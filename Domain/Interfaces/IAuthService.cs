using System.Threading.Tasks;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using System.Collections.Generic;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса аутентификации и авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Аутентификация пользователя по логину и паролю
        /// </summary>
        Task<User?> AuthenticateAsync(string username, string password, string? ipAddress = null);
        
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        Task<User> CreateUserAsync(User user, string password, string? ipAddress = null);
        
        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        Task<User> UpdateUserAsync(User user, string? ipAddress = null);
        
        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword, string? ipAddress = null);
        
        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);
        
        /// <summary>
        /// Проверка прав доступа пользователя на выполнение определенной операции
        /// </summary>
        bool HasPermission(User user, string operation);
        
        /// <summary>
        /// Генерация JWT токена
        /// </summary>
        Task<string> GenerateJwtTokenAsync(User user);
        
        /// <summary>
        /// Генерация Refresh токена
        /// </summary>
        Task<RefreshToken> GenerateRefreshTokenAsync(User user, string ipAddress);
        
        /// <summary>
        /// Обновление токена доступа с помощью Refresh токена
        /// </summary>
        Task<(string JwtToken, RefreshToken RefreshToken)> RefreshTokenAsync(string token, string ipAddress);
        
        /// <summary>
        /// Отзыв Refresh токена
        /// </summary>
        Task RevokeTokenAsync(string token, string ipAddress);
        
        /// <summary>
        /// Разблокировка заблокированного аккаунта
        /// </summary>
        Task UnlockUserAccountAsync(int userId, string? ipAddress = null);
        
        /// <summary>
        /// Получение журнала авторизаций для пользователя
        /// </summary>
        Task<IEnumerable<AuthLog>> GetUserAuthLogsAsync(int userId, int limit = 20);
        
        /// <summary>
        /// Настройка двухфакторной аутентификации
        /// </summary>
        Task<string> SetupTwoFactorAsync(int userId);
        
        /// <summary>
        /// Подтверждение двухфакторной аутентификации
        /// </summary>
        Task<bool> VerifyTwoFactorCodeAsync(int userId, string code);
        
        /// <summary>
        /// Отключение двухфакторной аутентификации
        /// </summary>
        Task DisableTwoFactorAsync(int userId, string? ipAddress = null);
    }
} 