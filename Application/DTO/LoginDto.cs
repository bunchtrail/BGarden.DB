namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO с данными для входа в систему
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; } = null!;
        
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = null!;
        
        /// <summary>
        /// IP-адрес пользователя (опционально)
        /// </summary>
        public string? IpAddress { get; set; }
        
        /// <summary>
        /// User-Agent браузера или клиента (опционально)
        /// </summary>
        public string? UserAgent { get; set; }
        
        /// <summary>
        /// Код подтверждения для двухфакторной аутентификации (опционально)
        /// </summary>
        public string? TwoFactorCode { get; set; }
        
        /// <summary>
        /// Запомнить пользователя (для длительной сессии)
        /// </summary>
        public bool RememberMe { get; set; }
    }
} 