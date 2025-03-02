namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи данных об экспозиции (Exposition)
    /// </summary>
    public class ExpositionDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Название экспозиции
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Дополнительное описание/характеристика
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Количество образцов в экспозиции (опционально)
        /// </summary>
        public int? SpecimensCount { get; set; }
    }
} 