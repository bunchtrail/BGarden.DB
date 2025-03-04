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
        /// Дата отзыва токена (null если не отозван)
        /// </summary>
        public DateTime? Revoked { get; set; }
        
        /// <summary>
        /// Причина отзыва токена
        /// </summary>
        public string? ReasonRevoked { get; set; }
        
        /// <summary>
        /// Дата создания токена
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата создания токена (альтернативное имя для совместимости)
        /// </summary>
        public DateTime Created { get => CreatedAt; set => CreatedAt = value; }
        
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
        public bool IsActive => Revoked == null && DateTime.UtcNow < ExpiryDate;
        
        /// <summary>
        /// Отозван ли токен (для обратной совместимости)
        /// </summary>
        public bool IsRevoked { get => Revoked != null; set { if (value) Revoked = DateTime.UtcNow; } }
    }
} 