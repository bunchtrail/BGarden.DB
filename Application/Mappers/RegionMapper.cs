using System.Linq;
using Application.DTO;
using BGarden.Domain.Entities;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Application.Mappers
{
    public static class RegionMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static RegionDto ToDto(this Region entity)
        {
            return new RegionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Radius = entity.Radius,
                BoundaryWkt = entity.Boundary?.AsText(),
                SectorType = entity.SectorType,
                SpecimensCount = entity.Specimens?.Count ?? 0
            };
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Region).
        /// </summary>
        public static Region ToEntity(this RegionDto dto)
        {
            var region = new Region
            {
                // Id = не задаём вручную, обычно автогенерируется
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Radius = dto.Radius,
                SectorType = dto.SectorType
            };

            // Установка географической точки
            region.Location = new Point((double)dto.Longitude, (double)dto.Latitude) { SRID = 4326 };

            // Попытка преобразовать WKT в Polygon, если он указан
            if (!string.IsNullOrEmpty(dto.BoundaryWkt))
            {
                try
                {
                    var wktReader = new WKTReader();
                    var geometry = wktReader.Read(dto.BoundaryWkt);
                    
                    if (geometry is Polygon polygon)
                    {
                        polygon.SRID = 4326;
                        region.Boundary = polygon;
                    }
                }
                catch
                {
                    // Игнорируем ошибки при парсинге WKT
                }
            }

            return region;
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this RegionDto dto, Region entity)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.Radius = dto.Radius;
            entity.SectorType = dto.SectorType;

            // Обновление географической точки
            entity.Location = new Point((double)dto.Longitude, (double)dto.Latitude) { SRID = 4326 };

            // Попытка преобразовать WKT в Polygon, если он указан
            if (!string.IsNullOrEmpty(dto.BoundaryWkt))
            {
                try
                {
                    var wktReader = new WKTReader();
                    var geometry = wktReader.Read(dto.BoundaryWkt);
                    
                    if (geometry is Polygon polygon)
                    {
                        polygon.SRID = 4326;
                        entity.Boundary = polygon;
                    }
                }
                catch
                {
                    // Игнорируем ошибки при парсинге WKT
                }
            }
            else
            {
                entity.Boundary = null;
            }
        }
    }
} 