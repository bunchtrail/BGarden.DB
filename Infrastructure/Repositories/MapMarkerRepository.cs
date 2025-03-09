using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Enums;
using BGarden.DB.Domain.Interfaces;
using BGarden.DB.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;
using BGarden.Infrastructure.Data;
using BGarden.Domain.Entities;
using NetTopologySuite.Geometries;

namespace BGarden.DB.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с маркерами на карте
    /// </summary>
    public class MapMarkerRepository : RepositoryBase<MapMarker>, IMapMarkerRepository
    {
        public MapMarkerRepository(BotanicalContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapMarker>> GetByTypeAsync(MarkerType type)
        {
            return await _dbSet
                .Where(m => m.Type == type)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MapMarker?> GetBySpecimenIdAsync(int specimenId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.SpecimenId == specimenId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapMarker>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng)
        {
            return await _dbSet
                .Where(m => m.Latitude >= southLat && m.Latitude <= northLat &&
                            m.Longitude >= westLng && m.Longitude <= eastLng)
                .ToListAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<MapMarker>> GetByRegionIdAsync(int regionId)
        {
            return await _dbSet
                .Where(m => m.RegionId == regionId)
                .ToListAsync();
        }
        
        /// <inheritdoc/>
        public async Task<MapMarker> CreateMarkerForSpecimenAsync(Specimen specimen)
        {
            // Создаем новый маркер для растения
            var marker = new MapMarker
            {
                Title = specimen.LatinName ?? specimen.RussianName ?? $"Растение #{specimen.Id}",
                Description = specimen.Notes ?? string.Empty,
                Latitude = specimen.Latitude.HasValue ? (double)specimen.Latitude.Value : 0,
                Longitude = specimen.Longitude.HasValue ? (double)specimen.Longitude.Value : 0,
                Type = MarkerType.Plant,
                SpecimenId = specimen.Id,
                RegionId = specimen.RegionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await _dbSet.AddAsync(marker);
            await _context.SaveChangesAsync();
            
            return marker;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapMarker>> FindNearbyAsync(Point point, double radiusInMeters)
        {
            // Временное решение: вместо пространственного поиска используем простой поиск по координатам
            // Приблизительное расстояние 1 градуса широты в метрах (на экваторе)
            const double metersPerDegreeLat = 111111;
            
            // Приблизительное расстояние 1 градуса долготы на заданной широте в метрах
            // (зависит от широты, уменьшается от экватора к полюсам)
            double latitude = point.Y;
            double longitude = point.X;
            double metersPerDegreeLon = metersPerDegreeLat * Math.Cos(latitude * Math.PI / 180);
            
            // Преобразуем радиус из метров в градусы для широты и долготы
            double latDelta = radiusInMeters / metersPerDegreeLat;
            double lonDelta = radiusInMeters / metersPerDegreeLon;
            
            // Ищем области, центр которых находится в пределах указанного прямоугольника
            return await _dbSet
                .Where(m => 
                    m.Latitude >= latitude - latDelta &&
                    m.Latitude <= latitude + latDelta &&
                    m.Longitude >= longitude - lonDelta &&
                    m.Longitude <= longitude + lonDelta)
                .ToListAsync();
        }
    }
} 