namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи данных о Family (семействе растений)
    /// </summary>
    public class FamilyDto
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Название семейства (латинское)
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Примечания, описание и т.п.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Количество образцов в семействе (опционально)
        /// </summary>
        public int? SpecimensCount { get; set; }
    }
} 