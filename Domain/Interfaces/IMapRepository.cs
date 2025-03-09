using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с картами
    /// </summary>
    public interface IMapRepository : IRepository<Map>
    {
        /// <summary>
        /// Получить карту по идентификатору вместе с растениями
        /// </summary>
        Task<Map> GetMapWithSpecimensAsync(int id);
        
        /// <summary>
        /// Получить все активные карты
        /// </summary>
        Task<IEnumerable<Map>> GetActiveMapsByAsync();
        
        /// <summary>
        /// Получить количество растений на карте
        /// </summary>
        Task<int> GetSpecimensCountByMapIdAsync(int mapId);
        
        /// <summary>
        /// Проверить, существует ли карта с указанным именем
        /// </summary>
        Task<bool> ExistsByNameAsync(string name);
    }
} 