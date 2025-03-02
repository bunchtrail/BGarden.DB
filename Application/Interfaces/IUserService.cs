using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Application.DTO;

namespace BGarden.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        Task<List<UserDto>> GetAllUsersAsync();
        
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        Task<UserDto?> GetUserByIdAsync(int id);
        
        /// <summary>
        /// Получение пользователя по имени
        /// </summary>
        Task<UserDto?> GetUserByUsernameAsync(string username);
        
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        
        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        
        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        
        /// <summary>
        /// Деактивация учетной записи пользователя
        /// </summary>
        Task DeactivateUserAsync(int id);
        
        /// <summary>
        /// Активация учетной записи пользователя
        /// </summary>
        Task ActivateUserAsync(int id);
        
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        Task<UserDto?> AuthenticateAsync(LoginDto loginDto);
    }
} 