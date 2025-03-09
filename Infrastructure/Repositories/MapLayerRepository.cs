using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы со слоями карты
    /// </summary>
    public class MapLayerRepository : RepositoryBase<MapLayer>, IMapLayerRepository
    {
        public MapLayerRepository(BotanicalContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<MapLayer> GetDefaultActiveLayerAsync()
        {
            return await _context.MapLayers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapLayer>> GetAllActiveLayersAsync()
        {
            return await _context.MapLayers
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MapLayer> FindByNameAsync(string name)
        {
            return await _context.MapLayers
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        /// <summary>
        /// Получение слоя с метаданными тайлов
        /// </summary>
        public async Task<MapLayer> GetByIdWithTilesAsync(int id)
        {
            return await _context.MapLayers
                .Include(x => x.TileMetadata)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
} 