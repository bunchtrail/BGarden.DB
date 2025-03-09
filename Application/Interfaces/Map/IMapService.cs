using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Application.DTO;
using BGarden.DB.Domain.Enums;

namespace Application.Interfaces.Map
{
    /// <summary>
    /// Интерфейс сервиса для работы с картой
    /// </summary>
    public interface IMapService
    {
        #region MapMarkers

        /// <summary>
        /// Получить все маркеры карты
        /// </summary>
        Task<IEnumerable<MapMarkerDto>> GetAllMarkersAsync();

        /// <summary>
        /// Получить маркер по идентификатору
        /// </summary>
        Task<MapMarkerDto> GetMarkerByIdAsync(int id);

        /// <summary>
        /// Получить маркеры по типу
        /// </summary>
        Task<IEnumerable<MapMarkerDto>> GetMarkersByTypeAsync(MarkerType type);

        /// <summary>
        /// Получить маркеры для конкретного экземпляра
        /// </summary>
        Task<IEnumerable<MapMarkerDto>> GetMarkersBySpecimenIdAsync(int specimenId);

        /// <summary>
        /// Создать новый маркер
        /// </summary>
        Task<MapMarkerDto> CreateMarkerAsync(CreateMapMarkerDto markerDto);

        /// <summary>
        /// Обновить существующий маркер
        /// </summary>
        Task<MapMarkerDto> UpdateMarkerAsync(UpdateMapMarkerDto markerDto);

        /// <summary>
        /// Удалить маркер по идентификатору
        /// </summary>
        Task<bool> DeleteMarkerAsync(int id);

        /// <summary>
        /// Найти ближайшие маркеры к указанной точке в пределах радиуса (в метрах)
        /// </summary>
        Task<IEnumerable<MapMarkerDto>> FindNearbyMarkersAsync(double latitude, double longitude, double radiusInMeters);

        #endregion

        #region MapAreas

        /// <summary>
        /// Получить все зоны карты
        /// </summary>
        Task<IEnumerable<MapAreaDto>> GetAllAreasAsync();

        /// <summary>
        /// Получить зону карты по идентификатору
        /// </summary>
        Task<MapAreaDto> GetAreaByIdAsync(int id);

        /// <summary>
        /// Получить зоны карты по типу
        /// </summary>
        Task<IEnumerable<MapAreaDto>> GetAreasByTypeAsync(AreaType type);

        /// <summary>
        /// Создать новую зону карты
        /// </summary>
        Task<MapAreaDto> CreateAreaAsync(CreateMapAreaDto areaDto);

        /// <summary>
        /// Обновить существующую зону карты
        /// </summary>
        Task<MapAreaDto> UpdateAreaAsync(UpdateMapAreaDto areaDto);

        /// <summary>
        /// Удалить зону карты по идентификатору
        /// </summary>
        Task<bool> DeleteAreaAsync(int id);

        #endregion

        #region MapOptions

        /// <summary>
        /// Получить все настройки карты
        /// </summary>
        Task<IEnumerable<MapOptionsDto>> GetAllOptionsAsync();

        /// <summary>
        /// Получить настройки карты по идентификатору
        /// </summary>
        Task<MapOptionsDto> GetOptionsByIdAsync(int id);

        /// <summary>
        /// Получить настройки карты по умолчанию
        /// </summary>
        Task<MapOptionsDto> GetDefaultOptionsAsync();

        /// <summary>
        /// Создать новые настройки карты
        /// </summary>
        Task<MapOptionsDto> CreateOptionsAsync(CreateMapOptionsDto optionsDto);

        /// <summary>
        /// Обновить существующие настройки карты
        /// </summary>
        Task<MapOptionsDto> UpdateOptionsAsync(UpdateMapOptionsDto optionsDto);

        /// <summary>
        /// Удалить настройки карты по идентификатору
        /// </summary>
        Task<bool> DeleteOptionsAsync(int id);

        #endregion

        #region MapLayers

        /// <summary>
        /// Получить все слои карты
        /// </summary>
        Task<IEnumerable<MapLayerDto>> GetAllLayersAsync();

        /// <summary>
        /// Получить слой карты по идентификатору
        /// </summary>
        Task<MapLayerDto> GetLayerByIdAsync(int id);

        /// <summary>
        /// Получить слой карты по умолчанию
        /// </summary>
        Task<MapLayerDto> GetDefaultLayerAsync();

        /// <summary>
        /// Создать новый слой карты
        /// </summary>
        Task<MapLayerDto> CreateLayerAsync(CreateMapLayerDto layerDto);

        /// <summary>
        /// Обновить существующий слой карты
        /// </summary>
        Task<MapLayerDto> UpdateLayerAsync(UpdateMapLayerDto layerDto);

        /// <summary>
        /// Удалить слой карты по идентификатору
        /// </summary>
        Task<bool> DeleteLayerAsync(int id);

        #endregion
    }
} 