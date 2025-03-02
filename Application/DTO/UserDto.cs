using System;
using BGarden.Domain.Enums;

namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для сущности пользователя системы
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Имя пользователя для входа в систему
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; } = null!;
        
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; } = null!;
        
        /// <summary>
        /// Роль пользователя в системе
        /// </summary>
        public UserRole Role { get; set; }
        
        /// <summary>
        /// Должность (только для работников ботанического сада)
        /// </summary>
        public string? Position { get; set; }
        
        /// <summary>
        /// Дата создания учетной записи
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата последнего входа в систему
        /// </summary>
        public DateTime? LastLogin { get; set; }
        
        /// <summary>
        /// Активна ли учетная запись
        /// </summary>
        public bool IsActive { get; set; }
    }
    
    /// <summary>
    /// DTO для создания нового пользователя
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Имя пользователя для входа в систему
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = null!;
        
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; } = null!;
        
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; } = null!;
        
        /// <summary>
        /// Роль пользователя в системе
        /// </summary>
        public UserRole Role { get; set; }
        
        /// <summary>
        /// Должность (только для работников ботанического сада)
        /// </summary>
        public string? Position { get; set; }
    }
    
    /// <summary>
    /// DTO для обновления данных пользователя
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; } = null!;
        
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; } = null!;
        
        /// <summary>
        /// Роль пользователя в системе
        /// </summary>
        public UserRole Role { get; set; }
        
        /// <summary>
        /// Должность (только для работников ботанического сада)
        /// </summary>
        public string? Position { get; set; }
        
        /// <summary>
        /// Активна ли учетная запись
        /// </summary>
        public bool IsActive { get; set; }
    }
    
    /// <summary>
    /// DTO для смены пароля пользователя
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// Текущий пароль
        /// </summary>
        public string CurrentPassword { get; set; } = null!;
        
        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; } = null!;
    }
    
    /// <summary>
    /// DTO для аутентификации пользователя
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = null!;
    }
} 