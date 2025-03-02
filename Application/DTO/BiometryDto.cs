using System;

namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи биометрических данных растений
    /// </summary>
    public class BiometryDto
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Ссылка на образец
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Информация об образце (опционально)
        /// </summary>
        public string? SpecimenInfo { get; set; }
        
        /// <summary>
        /// Дата измерения/наблюдения
        /// </summary>
        public DateTime MeasurementDate { get; set; }
        
        /// <summary>
        /// Высота растения (см)
        /// </summary>
        public float? Height { get; set; }
        
        /// <summary>
        /// Диаметр цветка (см)
        /// </summary>
        public float? FlowerDiameter { get; set; }
        
        /// <summary>
        /// Примечания/особые наблюдения
        /// </summary>
        public string? Notes { get; set; }
    }
} 