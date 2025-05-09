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
        /// URL для доступа к изображению. Формируется на стороне API.
        /// </summary>
        public string? ImageUrl { get; set; }
        
        /// <summary>
        /// Относительный путь к файлу (для внутреннего использования или отладки)
        /// </summary>
        public string? RelativeFilePath { get; set; }
        
        /// <summary>
        /// Исходное имя файла
        /// </summary>
        public string? OriginalFileName { get; set; }
        
        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long? FileSize { get; set; }
        
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
        /// Относительный путь к сохраненному файлу. Заполняется сервисом.
        /// </summary>
        public string FilePath { get; set; } = null!;
        
        /// <summary>
        /// Исходное имя файла (опционально)
        /// </summary>
        public string? OriginalFileName { get; set; }
        
        /// <summary>
        /// Размер файла в байтах (опционально)
        /// </summary>
        public long? FileSize { get; set; }
        
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
        public bool? IsMain { get; set; }
    }
} 