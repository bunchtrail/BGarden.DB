using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с метаданными тайлов карты
    /// </summary>
    public interface IMapTileMetadataRepository : IRepository<MapTileMetadata>
    {
        /// <summary>
        /// Получить метаданные тайла по координатам и масштабу
        /// </summary>
        /// <param name="layerId">Идентификатор слоя</param>
        /// <param name="zoom">Уровень масштабирования</param>
        /// <param name="x">Колонка (X координата)</param>
        /// <param name="y">Строка (Y координата)</param>
        /// <returns>Метаданные тайла или null</returns>
        Task<MapTileMetadata> GetTileMetadataAsync(int layerId, int zoom, int x, int y);

        /// <summary>
        /// Получить все метаданные тайлов для указанного слоя
        /// </summary>
        /// <param name="layerId">Идентификатор слоя</param>
        /// <returns>Коллекция метаданных тайлов</returns>
        Task<IEnumerable<MapTileMetadata>> GetAllTilesForLayerAsync(int layerId);

        /// <summary>
        /// Получить метаданные тайлов для указанного слоя и масштаба
        /// </summary>
        /// <param name="layerId">Идентификатор слоя</param>
        /// <param name="zoom">Уровень масштабирования</param>
        /// <returns>Коллекция метаданных тайлов</returns>
        Task<IEnumerable<MapTileMetadata>> GetTilesForZoomLevelAsync(int layerId, int zoom);
    }
} 