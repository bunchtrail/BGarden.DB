using Application.DTO;
using BGarden.Domain.Entities;

namespace Application.Mappers
{
    public static class BiometryMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static BiometryDto ToDto(this Biometry entity)
        {
            return new BiometryDto
            {
                Id = entity.Id,
                SpecimenId = entity.SpecimenId,
                SpecimenInfo = entity.Specimen?.InventoryNumber ?? entity.SpecimenId.ToString(),
                MeasurementDate = entity.MeasurementDate,
                Height = entity.Height,
                FlowerDiameter = entity.FlowerDiameter,
                Notes = entity.Notes
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Biometry).
        /// </summary>
        public static Biometry ToEntity(this BiometryDto dto)
        {
            return new Biometry
            {
                // Id = не задаём вручную, обычно автогенерируется
                SpecimenId = dto.SpecimenId,
                MeasurementDate = dto.MeasurementDate,
                Height = dto.Height,
                FlowerDiameter = dto.FlowerDiameter,
                Notes = dto.Notes
            };
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this BiometryDto dto, Biometry entity)
        {
            entity.SpecimenId = dto.SpecimenId;
            entity.MeasurementDate = dto.MeasurementDate;
            entity.Height = dto.Height;
            entity.FlowerDiameter = dto.FlowerDiameter;
            entity.Notes = dto.Notes;
        }
    }
} 