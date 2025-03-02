using Application.DTO;
using BGarden.Domain.Entities;

namespace Application.Mappers
{
    public static class SpecimenMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static SpecimenDto ToDto(this Specimen entity)
        {
            return new SpecimenDto
            {
                Id = entity.Id,
                InventoryNumber = entity.InventoryNumber,
                FamilyId = entity.FamilyId,
                FamilyName = entity.Family?.Name,
                Genus = entity.Genus,
                Species = entity.Species,
                Cultivar = entity.Cultivar,
                Form = entity.Form,
                ExpositionId = entity.ExpositionId,
                ExpositionName = entity.Exposition?.Name,
                HasHerbarium = entity.HasHerbarium,
                Notes = entity.Notes
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Specimen).
        /// </summary>
        public static Specimen ToEntity(this SpecimenDto dto)
        {
            return new Specimen
            {
                // Id = не задаём вручную, обычно автогенерируется
                InventoryNumber = dto.InventoryNumber,
                FamilyId = dto.FamilyId,
                Genus = dto.Genus,
                Species = dto.Species,
                Cultivar = dto.Cultivar,
                Form = dto.Form,
                ExpositionId = dto.ExpositionId,
                HasHerbarium = dto.HasHerbarium,
                Notes = dto.Notes
            };
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this SpecimenDto dto, Specimen entity)
        {
            entity.InventoryNumber = dto.InventoryNumber;
            entity.FamilyId = dto.FamilyId;
            entity.Genus = dto.Genus;
            entity.Species = dto.Species;
            entity.Cultivar = dto.Cultivar;
            entity.Form = dto.Form;
            entity.ExpositionId = dto.ExpositionId;
            entity.HasHerbarium = dto.HasHerbarium;
            entity.Notes = dto.Notes;
        }
    }
} 