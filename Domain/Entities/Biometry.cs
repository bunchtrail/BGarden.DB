namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Биометрические показатели, актуальные в основном для Цветоводства
    /// (но могут расширяться для других секторов).
    /// </summary>
    public class Biometry
    {
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на образец.
        /// </summary>
        public int SpecimenId { get; set; }
        public Specimen? Specimen { get; set; }

        /// <summary>
        /// Дата измерения/наблюдения.
        /// </summary>
        public DateTime MeasurementDate { get; set; }

        /// <summary>
        /// Пример: высота растения (см).
        /// </summary>
        public float? Height { get; set; }

        /// <summary>
        /// Пример: диаметр цветка (см).
        /// </summary>
        public float? FlowerDiameter { get; set; }

        /// <summary>
        /// Другие показатели — 
        /// (количество цветоносов, длина побегов, плотность куста и т.п.).
        /// </summary>

        /// <summary>
        /// Примечания/особые наблюдения.
        /// </summary>
        public string? Notes { get; set; }
    }
} 