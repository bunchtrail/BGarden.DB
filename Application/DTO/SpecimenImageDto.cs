namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи данных изображения образца
    /// </summary>
    public class SpecimenImageDto
    {
        /// <summary>
        /// Идентификатор изображения
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор образца
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Данные изображения в формате Base64
        /// </summary>
        public string? ImageDataBase64 { get; set; }
        
        /// <summary>
        /// Тип содержимого (MIME-тип)
        /// </summary>
        public string ContentType { get; set; } = "image/jpeg";
        
        /// <summary>
        /// Описание изображения
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Флаг, указывающий, является ли изображение основным
        /// </summary>
        public bool IsMain { get; set; }
        
        /// <summary>
        /// Дата и время загрузки изображения
        /// </summary>
        public DateTime UploadedAt { get; set; }
    }
    
    /// <summary>
    /// DTO для создания нового изображения образца
    /// </summary>
    public class CreateSpecimenImageDto
    {
        /// <summary>
        /// Идентификатор образца
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Данные изображения в формате Base64
        /// </summary>
        public string ImageDataBase64 { get; set; } = null!;
        
        /// <summary>
        /// Тип содержимого (MIME-тип)
        /// </summary>
        public string ContentType { get; set; } = "image/jpeg";
        
        /// <summary>
        /// Описание изображения
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Флаг, указывающий, является ли изображение основным
        /// </summary>
        public bool IsMain { get; set; }
    }
    
    /// <summary>
    /// DTO для обновления существующего изображения образца
    /// </summary>
    public class UpdateSpecimenImageDto
    {
        /// <summary>
        /// Описание изображения
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Флаг, указывающий, является ли изображение основным
        /// </summary>
        public bool IsMain { get; set; }
    }
} 