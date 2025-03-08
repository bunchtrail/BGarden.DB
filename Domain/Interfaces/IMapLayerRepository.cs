using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.Domain.Interfaces;

namespace BGarden.DB.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы со слоями карты
    /// </summary>
    public interface IMapLayerRepository : IRepository<MapLayer>
    {
        /// <summary>
        /// Получить слой карты по строковому идентификатору
        /// </summary>
        /// <param name="layerId">Строковый идентификатор слоя</param>
        /// <returns>Слой карты или null, если не найден</returns>
        Task<MapLayer> GetByLayerIdAsync(string layerId);

        /// <summary>
        /// Получить слой карты по умолчанию
        /// </summary>
        /// <returns>Слой карты по умолчанию или null, если таковых нет</returns>
        Task<MapLayer> GetDefaultAsync();

        /// <summary>
        /// Получить все слои карты, отсортированные по порядку отображения
        /// </summary>
        /// <returns>Отсортированный список слоев карты</returns>
        Task<IEnumerable<MapLayer>> GetAllOrderedAsync();

        /// <summary>
        /// Установить слой карты по умолчанию
        /// </summary>
        /// <param name="id">Идентификатор слоя, который нужно сделать используемым по умолчанию</param>
        /// <returns>Обновленный слой карты</returns>
        Task<MapLayer> SetDefaultAsync(int id);
    }
} 