using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с тайлами карты
    /// </summary>
    public interface IMapTileService
    {
        /// <summary>
        /// Получить тайл карты по координатам
        /// </summary>
        /// <param name="layerId">Идентификатор слоя карты</param>
        /// <param name="zoom">Уровень масштабирования</param>
        /// <param name="x">Колонка (X координата)</param>
        /// <param name="y">Строка (Y координата)</param>
        /// <returns>Поток с данными тайла или null, если тайл не найден</returns>
        Task<Stream> GetTileAsync(int layerId, int zoom, int x, int y);

        /// <summary>
        /// Создать новый слой карты из директории с тайлами
        /// </summary>
        /// <param name="name">Название слоя</param>
        /// <param name="description">Описание слоя</param>
        /// <param name="baseDirectory">Базовая директория с тайлами</param>
        /// <param name="isActive">Указывает, должен ли слой быть активным</param>
        /// <returns>Созданный слой карты</returns>
        Task<MapLayer> CreateMapLayerFromTilesAsync(string name, string description, string baseDirectory, bool isActive = true);

        /// <summary>
        /// Обработать изображение карты с помощью gdal2tiles и создать новый слой
        /// </summary>
        /// <param name="name">Название слоя</param>
        /// <param name="description">Описание слоя</param>
        /// <param name="inputImagePath">Путь к исходному изображению</param>
        /// <param name="outputDirectory">Директория для сохранения тайлов</param>
        /// <param name="minZoom">Минимальный уровень масштабирования</param>
        /// <param name="maxZoom">Максимальный уровень масштабирования</param>
        /// <param name="isActive">Указывает, должен ли слой быть активным</param>
        /// <returns>Созданный слой карты или null в случае ошибки</returns>
        Task<MapLayer> ProcessMapImageAsync(string name, string description, string inputImagePath, 
            string outputDirectory, int minZoom = 10, int maxZoom = 18, bool isActive = true);

        /// <summary>
        /// Обновить метаданные тайлов для указанного слоя
        /// </summary>
        /// <param name="layerId">Идентификатор слоя</param>
        /// <returns>Количество обновленных тайлов</returns>
        Task<int> RefreshTileMetadataAsync(int layerId);

        /// <summary>
        /// Получить все активные слои карты
        /// </summary>
        /// <returns>Коллекция активных слоев карты</returns>
        Task<IEnumerable<MapLayer>> GetAllActiveLayersAsync();
        
        /// <summary>
        /// Получить слой карты по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор слоя</param>
        /// <returns>Слой карты или null, если слой не найден</returns>
        Task<MapLayer> GetLayerByIdAsync(int id);
        
        /// <summary>
        /// Получить метаданные тайла
        /// </summary>
        /// <param name="layerId">Идентификатор слоя</param>
        /// <param name="zoom">Уровень масштабирования</param>
        /// <param name="x">Колонка (X координата)</param>
        /// <param name="y">Строка (Y координата)</param>
        /// <returns>Метаданные тайла или null, если тайл не найден</returns>
        Task<MapTileMetadata> GetTileMetadataAsync(int layerId, int zoom, int x, int y);
    }
} 