using System.Linq;
using Application.DTO;
using BGarden.Domain.Entities;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Application.Mappers
{
    /// <summary>
    /// Утилитарный класс для маппинга между доменной сущностью Region и DTO.
    /// </summary>
    public static class RegionMapper
    {
        /// <summary>
        /// Преобразует сущность Region в DTO.
        /// </summary>
        public static RegionDto ToDto(this Region region)
        {
            if (region == null)
                return null!;

            return new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                Latitude = region.Latitude,
                Longitude = region.Longitude,
                Radius = region.Radius,
                BoundaryWkt = region.BoundaryWkt,
                SectorType = region.SectorType
            };
        }

        /// <summary>
        /// Преобразует коллекцию Region в коллекцию DTO.
        /// </summary>
        public static IEnumerable<RegionDto> ToDto(this IEnumerable<Region> regions)
        {
            if (regions == null)
                return Enumerable.Empty<RegionDto>();

            return regions.Select(r => r.ToDto());
        }

        /// <summary>
        /// Обновляет существующую сущность Region данными из DTO.
        /// </summary>
        public static void UpdateFromDto(this Region region, RegionDto dto)
        {
            if (region == null || dto == null)
                return;

            region.Name = dto.Name;
            region.Description = dto.Description;
            region.Latitude = dto.Latitude;
            region.Longitude = dto.Longitude;
            region.Radius = dto.Radius;
            region.BoundaryWkt = dto.BoundaryWkt;
            region.SectorType = dto.SectorType;
        }

        /// <summary>
        /// Создаёт новую сущность Region из DTO.
        /// </summary>
        public static Region ToEntity(this RegionDto dto)
        {
            if (dto == null)
                return null!;

            return new Region
            {
                // Id не устанавливаем, его присвоит БД
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Radius = dto.Radius,
                BoundaryWkt = dto.BoundaryWkt,
                SectorType = dto.SectorType
            };
        }
    }
} 