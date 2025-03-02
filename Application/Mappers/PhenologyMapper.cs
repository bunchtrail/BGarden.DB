using Application.DTO;
using BGarden.Domain.Entities;

namespace Application.Mappers
{
    public static class PhenologyMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static PhenologyDto ToDto(this Phenology entity)
        {
            return new PhenologyDto
            {
                Id = entity.Id,
                SpecimenId = entity.SpecimenId,
                SpecimenInfo = entity.Specimen?.InventoryNumber ?? entity.SpecimenId.ToString(),
                Year = entity.Year,
                FloweringStart = entity.FloweringStart,
                FloweringEnd = entity.FloweringEnd,
                FruitingDate = entity.FruitingDate,
                Notes = entity.Notes
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Phenology).
        /// </summary>
        public static Phenology ToEntity(this PhenologyDto dto)
        {
            return new Phenology
            {
                // Id = не задаём вручную, обычно автогенерируется
                SpecimenId = dto.SpecimenId,
                Year = dto.Year,
                FloweringStart = dto.FloweringStart,
                FloweringEnd = dto.FloweringEnd,
                FruitingDate = dto.FruitingDate,
                Notes = dto.Notes
            };
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this PhenologyDto dto, Phenology entity)
        {
            entity.SpecimenId = dto.SpecimenId;
            entity.Year = dto.Year;
            entity.FloweringStart = dto.FloweringStart;
            entity.FloweringEnd = dto.FloweringEnd;
            entity.FruitingDate = dto.FruitingDate;
            entity.Notes = dto.Notes;
        }
    }
} 