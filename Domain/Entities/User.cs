using System;
using System.Collections.Generic;
using BGarden.Domain.Enums;

namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Сущность пользователя системы (работники и клиенты)
    /// </summary>
    public class User
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
        /// Хеш пароля пользователя
        /// </summary>
        public string PasswordHash { get; set; } = null!;
        
        /// <summary>
        /// Соль для хеширования пароля
        /// </summary>
        public string PasswordSalt { get; set; } = null!;
        
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
        /// Дата последней активности пользователя
        /// </summary>
        public DateTime? LastActiveAt { get; set; }
        
        /// <summary>
        /// Активна ли учетная запись
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Количество неудачных попыток входа
        /// </summary>
        public int FailedLoginAttempts { get; set; }
        
        /// <summary>
        /// Время блокировки аккаунта (если установлено)
        /// </summary>
        public DateTime? LockoutEnd { get; set; }
        
        /// <summary>
        /// Включена ли двухфакторная аутентификация
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
        
        /// <summary>
        /// Секретный ключ для двухфакторной аутентификации
        /// </summary>
        public string? TwoFactorKey { get; set; }
        
        /// <summary>
        /// Образцы, созданные или измененные данным пользователем (только для работников)
        /// </summary>
        public ICollection<Specimen>? ManagedSpecimens { get; set; }
    }
} 