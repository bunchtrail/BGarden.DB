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
        public async Task<IEnumerable<MapMarker>> GetBySpecimenIdAsync(int specimenId)
        {
            return await _dbSet
                .Where(m => m.SpecimenId == specimenId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapMarker>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng)
        {
            return await _dbSet
                .Where(m => m.Latitude >= southLat && m.Latitude <= northLat &&
                            m.Longitude >= westLng && m.Longitude <= eastLng)
                .ToListAsync();
        }
    }
} 