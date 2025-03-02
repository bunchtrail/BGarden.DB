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

        public int FamilyId { get; set; }
        public string? FamilyName { get; set; }  // Чтобы не тянуть всю сущность Family

        public string? Genus { get; set; }
        public string? Species { get; set; }
        public string? Cultivar { get; set; }
        public string? Form { get; set; }

        public int? ExpositionId { get; set; }
        public string? ExpositionName { get; set; }

        // Прочие поля по необходимости
        public bool HasHerbarium { get; set; }
        public string? Notes { get; set; }
    }
} 