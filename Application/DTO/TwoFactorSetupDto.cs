namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для настройки двухфакторной аутентификации
    /// </summary>
    public class TwoFactorSetupDto
    {
        /// <summary>
        /// Секретный ключ для настройки в приложении аутентификатора
        /// </summary>
        public string SecretKey { get; set; } = null!;
        
        /// <summary>
        /// URL для QR-кода
        /// </summary>
        public string QrCodeUrl { get; set; } = null!;
        
        /// <summary>
        /// Имя приложения для отображения в аутентификаторе
        /// </summary>
        public string AppName { get; set; } = "BGarden";
        
        /// <summary>
        /// Имя пользователя для отображения в аутентификаторе
        /// </summary>
        public string Username { get; set; } = null!;
    }
} 