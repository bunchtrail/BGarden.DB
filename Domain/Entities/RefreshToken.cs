using System;

namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Refresh-токен для продления сессии пользователя
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Уникальный идентификатор токена
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Значение токена
        /// </summary>
        public string Token { get; set; } = null!;
        
        /// <summary>
        /// Срок действия токена
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        
        /// <summary>
        /// Отозван ли токен
        /// </summary>
        public bool IsRevoked { get; set; }
        
        /// <summary>
        /// Дата создания токена
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// IP-адрес, с которого был создан токен
        /// </summary>
        public string? CreatedByIp { get; set; }
        
        /// <summary>
        /// IP-адрес, с которого токен был отозван
        /// </summary>
        public string? RevokedByIp { get; set; }
        
        /// <summary>
        /// Токен, который заменил текущий при ротации
        /// </summary>
        public string? ReplacedByToken { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя, которому принадлежит токен
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Связь с пользователем
        /// </summary>
        public User User { get; set; } = null!;
        
        /// <summary>
        /// Активен ли токен (не просрочен и не отозван)
        /// </summary>
        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiryDate;
    }
} 