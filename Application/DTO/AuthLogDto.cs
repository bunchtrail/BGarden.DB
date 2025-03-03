using System;

namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для записи журнала авторизации
    /// </summary>
    public class AuthLogDto
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Идентификатор пользователя (если известен)
        /// </summary>
        public int? UserId { get; set; }
        
        /// <summary>
        /// Тип события (числовое представление)
        /// </summary>
        public int EventTypeId { get; set; }
        
        /// <summary>
        /// Название типа события
        /// </summary>
        public string EventTypeName { get; set; } = null!;
        
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
    }
} 