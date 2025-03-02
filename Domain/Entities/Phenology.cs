namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Фенологические данные по годам для конкретного образца.
    /// </summary>
    public class Phenology
    {
        public int Id { get; set; }

        /// <summary>
        /// Связь с образцом.
        /// </summary>
        public int SpecimenId { get; set; }
        public Specimen? Specimen { get; set; }

        /// <summary>
        /// Год, за который ведутся фенологические наблюдения.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Дата начала цветения (пример).
        /// </summary>
        public DateTime? FloweringStart { get; set; }

        /// <summary>
        /// Дата окончания цветения (пример).
        /// </summary>
        public DateTime? FloweringEnd { get; set; }

        /// <summary>
        /// Дата плодоношения.
        /// </summary>
        public DateTime? FruitingDate { get; set; }

        /// <summary>
        /// И т.д. — при необходимости можно добавить поля 
        /// для любой другой фенофазы (распускание почек, листопад и т.п.).
        /// </summary>

        /// <summary>
        /// Комментарии/примечания.
        /// </summary>
        public string? Notes { get; set; }
    }
} 