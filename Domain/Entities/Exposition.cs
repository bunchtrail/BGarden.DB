namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Отдельная экспозиция (коллекция) в Ботаническом саду.
    /// Например, "Вересковый сад", "Альпийская горка" и т.п.
    /// </summary>
    public class Exposition
    {
        public int Id { get; set; }

        /// <summary>
        /// Название экспозиции.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Дополнительное описание/характеристика.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Связь: в этой экспозиции может быть несколько образцов.
        /// </summary>
        public ICollection<Specimen>? Specimens { get; set; }
    }
} 