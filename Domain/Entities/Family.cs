namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Сущность для хранения списка семейств.
    /// </summary>
    public class Family
    {
        public int Id { get; set; }

        /// <summary>
        /// Название семейства (латинское).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Примечания, описание и т.п.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Связь "семейство -> образцы".
        /// </summary>
        public ICollection<Specimen>? Specimens { get; set; }
    }
} 