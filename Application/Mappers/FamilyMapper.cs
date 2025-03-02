using Application.DTO;
using BGarden.Domain.Entities;
using System.Linq;

namespace Application.Mappers
{
    public static class FamilyMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static FamilyDto ToDto(this Family entity)
        {
            return new FamilyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SpecimensCount = entity.Specimens?.Count
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Family).
        /// </summary>
        public static Family ToEntity(this FamilyDto dto)
        {
            return new Family
            {
                // Id = не задаём вручную, обычно автогенерируется
                Name = dto.Name,
                Description = dto.Description
            };
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this FamilyDto dto, Family entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
        }
    }
} 