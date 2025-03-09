using System.Linq;
using System.Collections.Generic;
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
        private static readonly WKTReader _wktReader = new WKTReader();
        private static readonly WKTWriter _wktWriter = new WKTWriter();

        /// <summary>
        /// Преобразует сущность Region в DTO.
        /// </summary>
        public static RegionDto ToDto(this Region region)
        {
            if (region == null)
                return null!;

            var dto = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                Latitude = region.Latitude,
                Longitude = region.Longitude,
                Radius = region.Radius,
                BoundaryWkt = region.BoundaryWkt,
                SectorType = region.SectorType,
                PolygonCoordinates = region.PolygonCoordinates,
                StrokeColor = region.StrokeColor,
                FillColor = region.FillColor,
                FillOpacity = region.FillOpacity
            };

            // Если у нас есть пространственные данные, преобразуем их в WKT
            if (region.Boundary != null)
            {
                dto.BoundaryWkt = _wktWriter.Write(region.Boundary);
            }

            // Подсчет количества образцов
            dto.SpecimensCount = region.Specimens?.Count ?? 0;

            return dto;
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
            region.PolygonCoordinates = dto.PolygonCoordinates;
            region.StrokeColor = dto.StrokeColor;
            region.FillColor = dto.FillColor;
            region.FillOpacity = dto.FillOpacity;

            // Создаем геометрическую точку для центра региона
            region.Location = new Point(((double)dto.Longitude), ((double)dto.Latitude)) { SRID = 4326 };

            // Если у нас есть WKT границы, преобразуем его в полигон
            if (!string.IsNullOrEmpty(dto.BoundaryWkt))
            {
                try
                {
                    var geometry = _wktReader.Read(dto.BoundaryWkt);
                    if (geometry is Polygon polygon)
                    {
                        polygon.SRID = 4326; // WGS 84
                        region.Boundary = polygon;
                    }
                }
                catch
                {
                    // В случае ошибки при чтении WKT, оставляем Boundary как null
                    region.Boundary = null;
                }
            }
        }

        /// <summary>
        /// Создаёт новую сущность Region из DTO.
        /// </summary>
        public static Region ToEntity(this RegionDto dto)
        {
            if (dto == null)
                return null!;

            var region = new Region
            {
                // Id не устанавливаем, его присвоит БД
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Radius = dto.Radius,
                BoundaryWkt = dto.BoundaryWkt,
                SectorType = dto.SectorType,
                PolygonCoordinates = dto.PolygonCoordinates,
                StrokeColor = dto.StrokeColor,
                FillColor = dto.FillColor,
                FillOpacity = dto.FillOpacity,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            // Создаем геометрическую точку для центра региона
            region.Location = new Point(((double)dto.Longitude), ((double)dto.Latitude)) { SRID = 4326 };

            // Если у нас есть WKT границы, преобразуем его в полигон
            if (!string.IsNullOrEmpty(dto.BoundaryWkt))
            {
                try
                {
                    var geometry = _wktReader.Read(dto.BoundaryWkt);
                    if (geometry is Polygon polygon)
                    {
                        polygon.SRID = 4326; // WGS 84
                        region.Boundary = polygon;
                    }
                }
                catch
                {
                    // В случае ошибки при чтении WKT, оставляем Boundary как null
                    region.Boundary = null;
                }
            }

            return region;
        }
    }
} 