using Domain.Entities;
using NetTopologySuite.Geometries;


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
        /// Тип сектора, в котором находится образец.
        /// </summary>
        public BGarden.Domain.Enums.SectorType SectorType { get; set; }

        /// <summary>
        /// Координаты местоположения растения (широта).
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Координаты местоположения растения (долгота).
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Геометрическая точка, представляющая местоположение растения
        /// </summary>
        public Point? Location { get; set; }

        /// <summary>
        /// Идентификатор области, в которой находится растение.
        /// </summary>
        public int? RegionId { get; set; }
        
        /// <summary>
        /// Область, в которой находится растение.
        /// </summary>
        public Region? Region { get; set; }

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

        /// <summary>
        /// Идентификатор пользователя, который создал/отредактировал запись
        /// </summary>
        public int? CreatedByUserId { get; set; }
        
        /// <summary>
        /// Пользователь, который создал/отредактировал запись
        /// </summary>
        public User? CreatedByUser { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата последнего обновления записи
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// Идентификатор карты, на которой размещено растение
        /// </summary>
        public int? MapId { get; set; }
        
        /// <summary>
        /// Карта, на которой размещено растение
        /// </summary>
        public Map? Map { get; set; }
        
        /// <summary>
        /// Координата X на карте (в пикселях)
        /// </summary>
        public int? MapX { get; set; }
        
        /// <summary>
        /// Координата Y на карте (в пикселях)
        /// </summary>
        public int? MapY { get; set; }

        // Навигационные свойства для связи «один ко многим» со сторонними таблицами:
        public ICollection<Phenology>? Phenologies { get; set; }
        public ICollection<Biometry>? Biometries { get; set; }
        
        /// <summary>
        /// Коллекция изображений, связанных с образцом
        /// </summary>
        public ICollection<SpecimenImage>? SpecimenImages { get; set; }
    }
} 