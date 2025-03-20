using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Domain.Enums;
using BGarden.Infrastructure.Data;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace BGarden.Infrastructure.Repositories
{
    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(BotanicalContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await _context.Regions
                .Include(r => r.Specimens)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(int id)
        {
            return await _context.Regions
                .Include(r => r.Specimens)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Region>> GetRegionsBySectorTypeAsync(SectorType sectorType)
        {
            return await _context.Regions
                .AsNoTracking()
                .Where(r => r.SectorType == sectorType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Region>> GetRegionsWithSpecimensAsync()
        {
            return await _context.Regions
                .Include(r => r.Specimens)
                .Where(r => r.Specimens.Any())
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Region>> FindNearbyRegionsAsync(decimal latitude, decimal longitude, decimal radiusInMeters)
        {
            // Создаем точку из координат пользователя
            var userLocation = CreatePoint(longitude, latitude);

            // Используем пространственный запрос для поиска ближайших регионов
            if (userLocation != null)
            {
                return await _context.Regions
                    .AsNoTracking()
                    .Where(r => r.Location != null && r.Location.Distance(userLocation) <= (double)radiusInMeters)
                    .ToListAsync();
            }
            else
            {
                // Если не удалось создать точку, используем старый метод поиска
                return await FindNearbyRegionsLegacyAsync(latitude, longitude, radiusInMeters);
            }
        }

        public async Task<bool> IsPointInRegionAsync(int regionId, decimal latitude, decimal longitude)
        {
            // Создаем точку из координат
            var point = CreatePoint(longitude, latitude);
            if (point == null) return false;

            // Получаем регион
            var region = await _context.Regions
                .FirstOrDefaultAsync(r => r.Id == regionId);
                
            if (region == null || string.IsNullOrEmpty(region.PolygonCoordinates)) 
                return false;
                
            try
            {
                // Создаем полигон из координат
                var polygon = CreatePolygonFromCoordinates(region.PolygonCoordinates);
                if (polygon == null) return false;
                
                // Проверяем, находится ли точка внутри полигона
                return polygon.Contains(point);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Specimen>> GetSpecimensInRegionAsync(int regionId)
        {
            return await _context.Specimens
                .Where(s => s.RegionId == regionId)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task UpdateRegionPolygonAsync(int regionId, string polygonCoordinates, string? strokeColor = null, string? fillColor = null, decimal? fillOpacity = null)
        {
            var region = await _context.Regions
                .FirstOrDefaultAsync(r => r.Id == regionId);
                
            if (region != null)
            {
                region.PolygonCoordinates = polygonCoordinates;
                
                if (!string.IsNullOrEmpty(strokeColor))
                    region.StrokeColor = strokeColor;
                    
                if (!string.IsNullOrEmpty(fillColor))
                    region.FillColor = fillColor;
                    
                if (fillOpacity.HasValue)
                    region.FillOpacity = fillOpacity;
                    
                region.UpdatedAt = DateTime.UtcNow;
                
                try
                {
                    // Попытка создать полигон для проверки корректности координат
                    var polygon = CreatePolygonFromCoordinates(polygonCoordinates);
                    if (polygon != null)
                    {
                        // Преобразуем полигон в WKT и сохраняем в Boundary
                        region.Boundary = polygon;
                    }
                }
                catch (Exception)
                {
                    // Если координаты некорректны, оставляем только текстовое представление
                }
                
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Устаревший метод поиска ближайших регионов (без использования пространственных типов)
        /// </summary>
        private async Task<IEnumerable<Region>> FindNearbyRegionsLegacyAsync(decimal latitude, decimal longitude, decimal radiusInMeters)
        {
            // Приблизительное расстояние 1 градуса широты в метрах (на экваторе)
            const decimal metersPerDegreeLat = 111111;
            
            // Приблизительное расстояние 1 градуса долготы на заданной широте в метрах
            // (зависит от широты, уменьшается от экватора к полюсам)
            decimal metersPerDegreeLon = metersPerDegreeLat * (decimal)Math.Cos((double)latitude * Math.PI / 180);
            
            // Преобразуем радиус из метров в градусы для широты и долготы
            decimal latDelta = radiusInMeters / metersPerDegreeLat;
            decimal lonDelta = radiusInMeters / metersPerDegreeLon;
            
            // Ищем области, центр которых находится в пределах указанного прямоугольника
            var nearbyRegions = await _context.Regions
                .AsNoTracking()
                .Where(r => 
                    r.Latitude >= latitude - latDelta &&
                    r.Latitude <= latitude + latDelta &&
                    r.Longitude >= longitude - lonDelta &&
                    r.Longitude <= longitude + lonDelta)
                .ToListAsync();
            
            // Для более точного поиска отфильтровываем результаты,
            // вычислив точное расстояние по формуле гаверсинусов
            return nearbyRegions.Where(r => CalculateDistance(
                (double)latitude, (double)longitude, 
                (double)r.Latitude, (double)r.Longitude) <= (double)radiusInMeters);
        }
        
        /// <summary>
        /// Вычисляет расстояние между двумя точками на земной поверхности (по формуле гаверсинусов)
        /// </summary>
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371000; // Радиус Земли в метрах
            
            // Конвертация в радианы
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            
            // Формула гаверсинусов
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            
            return earthRadius * c; // Расстояние в метрах
        }

        /// <summary>
        /// Создает точку из координат (долгота, широта)
        /// </summary>
        private Point? CreatePoint(decimal longitude, decimal latitude)
        {
            try
            {
                // SRID = 4326 - WGS84, стандартная система координат для GPS
                return new Point((double)longitude, (double)latitude) { SRID = 4326 };
            }
            catch (Exception)
            {
                // В случае ошибки возвращаем null
                return null;
            }
        }
        
        /// <summary>
        /// Создает полигон из строки координат в формате GeoJSON
        /// </summary>
        private Polygon? CreatePolygonFromCoordinates(string coordinatesJson)
        {
            try
            {
                // Парсим массив координат формата [[lon1,lat1],[lon2,lat2],...]
                var coordinates = JsonConvert.DeserializeObject<double[][]>(coordinatesJson);
                if (coordinates == null || coordinates.Length < 3) 
                    return null;
                
                // Создаем координаты для полигона
                var points = coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray();
                
                // Для полигона последняя точка должна совпадать с первой
                if (!points.First().Equals2D(points.Last()))
                {
                    var newPoints = new Coordinate[points.Length + 1];
                    Array.Copy(points, newPoints, points.Length);
                    newPoints[points.Length] = points[0];
                    points = newPoints;
                }
                
                // Создаем полигон
                var factory = new GeometryFactory(new PrecisionModel(), 4326);
                var ring = factory.CreateLinearRing(points);
                return factory.CreatePolygon(ring);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
} 