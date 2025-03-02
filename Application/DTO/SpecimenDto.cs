namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи данных о Specimen 
    /// (например, наружу в будущий UI или во внутренние сервисы).
    /// </summary>
    public class SpecimenDto
    {
        public int Id { get; set; }
        public string InventoryNumber { get; set; } = null!;

        public BGarden.Domain.Enums.SectorType SectorType { get; set; }
        
        /// <summary>
        /// Координаты местоположения растения (широта)
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Координаты местоположения растения (долгота)
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Идентификатор области (региона), в которой находится растение
        /// </summary>
        public int? RegionId { get; set; }
        
        /// <summary>
        /// Название области (региона), в которой находится растение
        /// </summary>
        public string? RegionName { get; set; }

        public int FamilyId { get; set; }
        public string? FamilyName { get; set; }  // Чтобы не тянуть всю сущность Family

        public string? RussianName { get; set; }
        public string? LatinName { get; set; }
        public string? Genus { get; set; }
        public string? Species { get; set; }
        public string? Cultivar { get; set; }
        public string? Form { get; set; }
        public string? Synonyms { get; set; }
        public string? DeterminedBy { get; set; }
        public int? PlantingYear { get; set; }
        public string? SampleOrigin { get; set; }
        public string? NaturalRange { get; set; }
        public string? EcologyAndBiology { get; set; }
        public string? EconomicUse { get; set; }
        public string? ConservationStatus { get; set; }

        public int? ExpositionId { get; set; }
        public string? ExpositionName { get; set; }

        public bool HasHerbarium { get; set; }
        public string? DuplicatesInfo { get; set; }
        public string? OriginalBreeder { get; set; }
        public int? OriginalYear { get; set; }
        public string? Country { get; set; }
        public string? Illustration { get; set; }
        public string? Notes { get; set; }
        public string? FilledBy { get; set; }
    }
} 