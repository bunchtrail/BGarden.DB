using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGarden.Application.DTO;
using BGarden.Application.Interfaces;
using BGarden.Application.Mappers;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;

namespace BGarden.Application.Services
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        
        public UserService(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(u => u.ToDto()).ToList();
        }
        
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user?.ToDto();
        }
        
        /// <summary>
        /// Получение пользователя по имени
        /// </summary>
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user?.ToDto();
        }
        
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Проверка на существование пользователя с таким же именем
            if (await _userRepository.ExistsByUsernameAsync(createUserDto.Username))
                throw new InvalidOperationException($"Пользователь с именем {createUserDto.Username} уже существует");
                
            // Проверка на существование пользователя с таким же email
            if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
                throw new InvalidOperationException($"Пользователь с email {createUserDto.Email} уже существует");
                
            // Преобразование DTO в сущность
            var user = createUserDto.ToEntity();
            
            // Создание пользователя через сервис аутентификации для корректной обработки пароля
            var createdUser = await _authService.CreateUserAsync(user, createUserDto.Password);
            
            // Сохранение изменений
            await _unitOfWork.SaveChangesAsync();
            
            return createdUser.ToDto();
        }
        
        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                throw new InvalidOperationException($"Пользователь с ID {id} не найден");
                
            // Обновление данных пользователя
            user.UpdateFromDto(updateUserDto);
            
            // Обновление через сервис аутентификации
            var updatedUser = await _authService.UpdateUserAsync(user);
            
            // Сохранение изменений
            await _unitOfWork.SaveChangesAsync();
            
            return updatedUser.ToDto();
        }
        
        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        public async Task ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            await _authService.ChangePasswordAsync(
                userId, 
                changePasswordDto.CurrentPassword, 
                changePasswordDto.NewPassword);
                
            await _unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Деактивация учетной записи пользователя
        /// </summary>
        public async Task DeactivateUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                throw new InvalidOperationException($"Пользователь с ID {id} не найден");
                
            user.IsActive = false;
            
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Активация учетной записи пользователя
        /// </summary>
        public async Task ActivateUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                throw new InvalidOperationException($"Пользователь с ID {id} не найден");
                
            user.IsActive = true;
            
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        public async Task<UserDto?> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            return user?.ToDto();
        }
    }
} 