using System;

namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для токенов аутентификации
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// JWT-токен доступа
        /// </summary>
        public string AccessToken { get; set; } = null!;
        
        /// <summary>
        /// Refresh-токен для обновления JWT
        /// </summary>
        public string RefreshToken { get; set; } = null!;
        
        /// <summary>
        /// Срок действия токена
        /// </summary>
        public DateTime Expiration { get; set; }
        
        /// <summary>
        /// Тип токена
        /// </summary>
        public string TokenType { get; set; } = "Bearer";
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Требуется ли двухфакторная аутентификация
        /// </summary>
        public bool RequiresTwoFactor { get; set; }
    }
} 