using BGarden.Domain.Entities;

namespace Domain.Entities
{
    /// <summary>
    /// Сущность для хранения изображений образцов растений
    /// </summary>
    public class SpecimenImage
    {
        /// <summary>
        /// Уникальный идентификатор (первичный ключ)
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор образца растения
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Навигационное свойство к образцу растения
        /// </summary>
        public Specimen Specimen { get; set; } = null!;
        
        /// <summary>
        /// Относительный путь к файлу изображения в файловой системе.
        /// Например: "/specimen-images/00001/image_abc.jpg"
        /// </summary>
        public string FilePath { get; set; } = null!;
        
        /// <summary>
        /// Исходное имя файла (опционально, для информации)
        /// </summary>
        public string? OriginalFileName { get; set; }
        
        /// <summary>
        /// Размер файла в байтах (опционально, для информации)
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
} 