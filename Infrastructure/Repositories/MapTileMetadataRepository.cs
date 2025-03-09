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
    /// Репозиторий для работы с метаданными тайлов карты
    /// </summary>
    public class MapTileMetadataRepository : RepositoryBase<MapTileMetadata>, IMapTileMetadataRepository
    {
        public MapTileMetadataRepository(BotanicalContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<MapTileMetadata> GetTileMetadataAsync(int layerId, int zoom, int x, int y)
        {
            return await _context.MapTileMetadata
                .FirstOrDefaultAsync(t => 
                    t.MapLayerId == layerId && 
                    t.ZoomLevel == zoom && 
                    t.TileColumn == x && 
                    t.TileRow == y);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapTileMetadata>> GetAllTilesForLayerAsync(int layerId)
        {
            return await _context.MapTileMetadata
                .Where(t => t.MapLayerId == layerId)
                .OrderBy(t => t.ZoomLevel)
                .ThenBy(t => t.TileColumn)
                .ThenBy(t => t.TileRow)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapTileMetadata>> GetTilesForZoomLevelAsync(int layerId, int zoom)
        {
            return await _context.MapTileMetadata
                .Where(t => t.MapLayerId == layerId && t.ZoomLevel == zoom)
                .OrderBy(t => t.TileColumn)
                .ThenBy(t => t.TileRow)
                .ToListAsync();
        }

        /// <summary>
        /// Получить количество тайлов для указанного слоя
        /// </summary>
        public async Task<int> GetTileCountForLayerAsync(int layerId)
        {
            return await _context.MapTileMetadata
                .CountAsync(t => t.MapLayerId == layerId);
        }
    }
} 