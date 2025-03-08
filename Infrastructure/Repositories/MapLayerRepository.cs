using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Interfaces;
using BGarden.DB.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;
using BGarden.Infrastructure.Data;
namespace BGarden.DB.Infrastructure.Repositories
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
        public async Task<MapLayer> GetByLayerIdAsync(string layerId)
        {
            return await _dbSet
                .Where(l => l.LayerId == layerId)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<MapLayer> GetDefaultAsync()
        {
            return await _dbSet
                .Where(l => l.IsDefault)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapLayer>> GetAllOrderedAsync()
        {
            return await _dbSet
                .OrderBy(l => l.DisplayOrder)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<MapLayer> SetDefaultAsync(int id)
        {
            // Начинаем транзакцию
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Сначала сбрасываем флаг IsDefault для всех слоев
                var allLayers = await _dbSet.ToListAsync();
                foreach (var layer in allLayers)
                {
                    layer.IsDefault = false;
                }

                // Затем устанавливаем флаг IsDefault для указанного слоя
                var targetLayer = await _dbSet.FindAsync(id);
                if (targetLayer == null)
                {
                    throw new ArgumentException($"Слой карты с ID {id} не найден");
                }

                targetLayer.IsDefault = true;
                targetLayer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return targetLayer;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
} 