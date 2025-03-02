namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Основная сущность, описывающая образец (растение) в коллекции.
    /// </summary>
    public class Specimen
    {
        /// <summary>
        /// Уникальный идентификатор (первичный ключ).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Уникальный инвентарный номер, совпадает с номером в регистрационном журнале.
        /// </summary>
        public string InventoryNumber { get; set; } = null!;

        /// <summary>
        /// Семейство (ссылка на сущность Family).
        /// </summary>
        public int FamilyId { get; set; }
        public Family? Family { get; set; }

        /// <summary>
        /// Русское название растения.
        /// </summary>
        public string? RussianName { get; set; }
        
        /// <summary>
        /// Латинское название растения.
        /// </summary>
        public string? LatinName { get; set; }

        /// <summary>
        /// Род (например, Rhododendron).
        /// </summary>
        public string? Genus { get; set; }

        /// <summary>
        /// Вид (например, Rhododendron luteum).
        /// </summary>
        public string? Species { get; set; }

        /// <summary>
        /// Сорт (если есть, напр. 'Golden Lights').
        /// </summary>
        public string? Cultivar { get; set; }

        /// <summary>
        /// Форма (например, декоративная форма вида).
        /// </summary>
        public string? Form { get; set; }

        /// <summary>
        /// Синонимы латинских названий (при наличии).
        /// </summary>
        public string? Synonyms { get; set; }

        /// <summary>
        /// Кто определил (выявил) вид, сорт и т.п.
        /// </summary>
        public string? DeterminedBy { get; set; }

        /// <summary>
        /// Год посадки (или год внесения в коллекцию).
        /// </summary>
        public int? PlantingYear { get; set; }

        /// <summary>
        /// Происхождение образца (источник, питомник, коллекция).
        /// </summary>
        public string? SampleOrigin { get; set; }

        /// <summary>
        /// Природный ареал произрастания (если вид дикорастущий).
        /// </summary>
        public string? NaturalRange { get; set; }

        /// <summary>
        /// Краткие сведения об экологии и биологии вида.
        /// </summary>
        public string? EcologyAndBiology { get; set; }

        /// <summary>
        /// Хозяйственное (практическое) применение.
        /// </summary>
        public string? EconomicUse { get; set; }

        /// <summary>
        /// Охранный статус (например, Красная Книга, МСОП).
        /// </summary>
        public string? ConservationStatus { get; set; }

        /// <summary>
        /// Местоположение (ссылка на сущность Exposition).
        /// </summary>
        public int? ExpositionId { get; set; }
        public Exposition? Exposition { get; set; }

        /// <summary>
        /// Наличие гербария (true/false).
        /// </summary>
        public bool HasHerbarium { get; set; }

        /// <summary>
        /// Информация о наличии дубликатов в других гербариях.
        /// Можно сделать отдельное булево поле,
        /// но в описании фигурирует как текстовое.
        /// </summary>
        public string? DuplicatesInfo { get; set; }

        /// <summary>
        /// Оригинатор (для сортов).
        /// </summary>
        public string? OriginalBreeder { get; set; }

        /// <summary>
        /// Год селекции или регистрации сорта.
        /// </summary>
        public int? OriginalYear { get; set; }

        /// <summary>
        /// Страна происхождения или селекции.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Иллюстрация (путь к изображению, URL и т.п.).
        /// </summary>
        public string? Illustration { get; set; }

        /// <summary>
        /// Примечания (любой объём).
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Кто заполнил запись, ФИО, должность и т.п.
        /// </summary>
        public string? FilledBy { get; set; }

        // Навигационные свойства для связи «один ко многим» со сторонними таблицами:
        public ICollection<Phenology>? Phenologies { get; set; }
        public ICollection<Biometry>? Biometries { get; set; }
    }
} 