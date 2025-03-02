using System;

namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи фенологических данных
    /// </summary>
    public class PhenologyDto
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Связь с образцом
        /// </summary>
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Информация об образце (опционально)
        /// </summary>
        public string? SpecimenInfo { get; set; }
        
        /// <summary>
        /// Год, за который ведутся фенологические наблюдения
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// Дата начала цветения
        /// </summary>
        public DateTime? FloweringStart { get; set; }
        
        /// <summary>
        /// Дата окончания цветения
        /// </summary>
        public DateTime? FloweringEnd { get; set; }
        
        /// <summary>
        /// Дата плодоношения
        /// </summary>
        public DateTime? FruitingDate { get; set; }
        
        /// <summary>
        /// Комментарии/примечания
        /// </summary>
        public string? Notes { get; set; }
    }
} 