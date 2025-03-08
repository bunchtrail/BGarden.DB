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

namespace BGarden.DB.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с областями на карте
    /// </summary>
    public class MapAreaRepository : RepositoryBase<MapArea>, IMapAreaRepository
    {
        public MapAreaRepository(BotanicalContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public override async Task<MapArea> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Coordinates.OrderBy(c => c.Order))
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<MapArea>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.Coordinates.OrderBy(c => c.Order))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapArea>> GetByTypeAsync(AreaType type)
        {
            return await _dbSet
                .Include(a => a.Coordinates.OrderBy(c => c.Order))
                .Where(a => a.Type == type)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapArea>> GetByExpositionIdAsync(int expositionId)
        {
            return await _dbSet
                .Include(a => a.Coordinates.OrderBy(c => c.Order))
                .Where(a => a.ExpositionId == expositionId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapArea>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng)
        {
            // Получаем области, у которых хотя бы одна координата находится в указанном прямоугольнике
            // Это не идеальное решение для определения пересечения полигонов, но для простых случаев подойдет
            var areaIds = await _context.Set<MapAreaCoordinate>()
                .Where(c => c.Latitude >= southLat && c.Latitude <= northLat &&
                            c.Longitude >= westLng && c.Longitude <= eastLng)
                .Select(c => c.MapAreaId)
                .Distinct()
                .ToListAsync();

            return await _dbSet
                .Include(a => a.Coordinates.OrderBy(c => c.Order))
                .Where(a => areaIds.Contains(a.Id))
                .ToListAsync();
        }
    }
} 