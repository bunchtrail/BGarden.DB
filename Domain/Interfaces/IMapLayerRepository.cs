using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы со слоями карты
    /// </summary>
    public interface IMapLayerRepository : IRepository<MapLayer>
    {
        /// <summary>
        /// Получить активный слой карты по умолчанию
        /// </summary>
        /// <returns>Активный слой карты или null</returns>
        Task<MapLayer> GetDefaultActiveLayerAsync();

        /// <summary>
        /// Получить все активные слои карты
        /// </summary>
        /// <returns>Коллекция активных слоев карты</returns>
        Task<IEnumerable<MapLayer>> GetAllActiveLayersAsync();

        /// <summary>
        /// Найти слой карты по имени
        /// </summary>
        /// <param name="name">Название слоя</param>
        /// <returns>Слой карты или null</returns>
        Task<MapLayer> FindByNameAsync(string name);
    }
} 