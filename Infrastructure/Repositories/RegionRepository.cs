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
    }
} 