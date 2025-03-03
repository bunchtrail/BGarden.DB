using System.ComponentModel.DataAnnotations;
using BGarden.Domain.Enums;

namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO модель для регистрации пользователя
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Электронная почта
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        /// <summary>
        /// Роль пользователя (по умолчанию Client)
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Client;
        
        /// <summary>
        /// IP-адрес пользователя
        /// </summary>
        public string? IpAddress { get; set; }
        
        /// <summary>
        /// User-Agent пользователя
        /// </summary>
        public string? UserAgent { get; set; }
    }
} 