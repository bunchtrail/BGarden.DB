using System;
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
    /// Репозиторий для работы с настройками карты
    /// </summary>
    public class MapOptionsRepository : RepositoryBase<MapOptions>, IMapOptionsRepository
    {
        public MapOptionsRepository(BotanicalContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<MapOptions> GetDefaultAsync()
        {
            return await _dbSet
                .Where(o => o.IsDefault)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<MapOptions> SetDefaultAsync(int id)
        {
            // Начинаем транзакцию
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Сначала сбрасываем флаг IsDefault для всех настроек
                var allOptions = await _dbSet.ToListAsync();
                foreach (var option in allOptions)
                {
                    option.IsDefault = false;
                }

                // Затем устанавливаем флаг IsDefault для указанных настроек
                var targetOption = await _dbSet.FindAsync(id);
                if (targetOption == null)
                {
                    throw new ArgumentException($"Настройки карты с ID {id} не найдены");
                }

                targetOption.IsDefault = true;
                targetOption.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return targetOption;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
} 