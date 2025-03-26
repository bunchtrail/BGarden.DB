using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    /// <summary>
    /// DTO для создания нового изображения образца с поддержкой бинарных данных
    /// </summary>
    public class CreateSpecimenImageBinaryDto
    {
        /// <summary>
        /// Идентификатор образца
        /// </summary>
        [Required]
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Бинарные данные изображения
        /// </summary>
        [Required]
        public byte[] ImageData { get; set; } = null!;
        
        /// <summary>
        /// Тип содержимого (MIME-тип)
        /// </summary>
        [Required]
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
} 