using System;
using BGarden.Domain.Enums;

namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Журнал событий аутентификации и авторизации
    /// </summary>
    public class AuthLog
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Имя пользователя, связанного с событием
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Идентификатор пользователя (если известен)
        /// </summary>
        public int? UserId { get; set; }
        
        /// <summary>
        /// Тип события
        /// </summary>
        public AuthEventType EventType { get; set; }
        
        /// <summary>
        /// IP-адрес, с которого выполнялось действие
        /// </summary>
        public string? IpAddress { get; set; }
        
        /// <summary>
        /// User-Agent браузера
        /// </summary>
        public string? UserAgent { get; set; }
        
        /// <summary>
        /// Дата и время события
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Успешно ли выполнено действие
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Дополнительные сведения о событии
        /// </summary>
        public string? Details { get; set; }
        
        /// <summary>
        /// Связь с пользователем
        /// </summary>
        public User? User { get; set; }
    }
} 