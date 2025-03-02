using System.Threading.Tasks;
using BGarden.Domain.Entities;

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
        Task<User?> AuthenticateAsync(string username, string password);
        
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        Task<User> CreateUserAsync(User user, string password);
        
        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        Task<User> UpdateUserAsync(User user);
        
        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        
        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);
        
        /// <summary>
        /// Проверка прав доступа пользователя на выполнение определенной операции
        /// </summary>
        bool HasPermission(User user, string operation);
    }
} 