using Application.DTO;
using BGarden.Domain.Entities;
using System.Linq;

namespace Application.Mappers
{
    public static class ExpositionMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static ExpositionDto ToDto(this Exposition entity)
        {
            return new ExpositionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SpecimensCount = entity.Specimens?.Count
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Exposition).
        /// </summary>
        public static Exposition ToEntity(this ExpositionDto dto)
        {
            return new Exposition
            {
                // Id = не задаём вручную, обычно автогенерируется
                Name = dto.Name,
                Description = dto.Description
            };
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this ExpositionDto dto, Exposition entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
        }
    }
} 