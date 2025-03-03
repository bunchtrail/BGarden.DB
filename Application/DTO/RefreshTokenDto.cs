namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для работы с токеном обновления
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; } = null!;
        
        /// <summary>
        /// IP-адрес, с которого запрашивается обновление токена
        /// </summary>
        public string? IpAddress { get; set; }
    }
} 