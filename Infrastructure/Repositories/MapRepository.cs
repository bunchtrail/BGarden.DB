using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGarden.Domain.Interfaces;
using Domain.Entities;
using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с картами
    /// </summary>
    public class MapRepository : RepositoryBase<Map>, IMapRepository
    {
        public MapRepository(BotanicalContext context) : base(context)
        {
        }

        /// <summary>
        /// Получить карту по идентификатору вместе с растениями
        /// </summary>
        public async Task<Map> GetMapWithSpecimensAsync(int id)
        {
            return await _context.Maps
                .Include(m => m.Specimens)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Получить все активные карты
        /// </summary>
        public async Task<IEnumerable<Map>> GetActiveMapsByAsync()
        {
            return await _context.Maps
                .Where(m => m.IsActive)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Получить количество растений на карте
        /// </summary>
        public async Task<int> GetSpecimensCountByMapIdAsync(int mapId)
        {
            return await _context.Specimens
                .CountAsync(s => s.MapId == mapId);
        }

        /// <summary>
        /// Проверить, существует ли карта с указанным именем
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Maps
                .AnyAsync(m => m.Name == name);
        }
    }
} 