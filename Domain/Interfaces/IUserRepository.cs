using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с пользователями
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Получение пользователя по имени пользователя
        /// </summary>
        Task<User?> GetByUsernameAsync(string username);
        
        /// <summary>
        /// Проверка существования пользователя с указанным именем
        /// </summary>
        Task<bool> ExistsByUsernameAsync(string username);
        
        /// <summary>
        /// Проверка существования пользователя с указанным email
        /// </summary>
        Task<bool> ExistsByEmailAsync(string email);
        
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        Task<List<User>> GetAllUsersAsync();
    }
} 