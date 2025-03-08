using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Enums;
using BGarden.Domain.Interfaces;
using BGarden.Domain.Entities;

namespace BGarden.DB.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с маркерами карты
    /// </summary>
    public interface IMapMarkerRepository : IRepository<MapMarker>
    {
        /// <summary>
        /// Получить маркеры по типу
        /// </summary>
        /// <param name="type">Тип маркера</param>
        /// <returns>Список маркеров указанного типа</returns>
        Task<IEnumerable<MapMarker>> GetByTypeAsync(MarkerType type);

        /// <summary>
        /// Получить маркер, связанный с указанным экземпляром растения
        /// </summary>
        /// <param name="specimenId">Идентификатор экземпляра растения</param>
        /// <returns>Маркер, связанный с указанным экземпляром</returns>
        Task<MapMarker?> GetBySpecimenIdAsync(int specimenId);

        /// <summary>
        /// Получить маркеры в указанном географическом прямоугольнике
        /// </summary>
        /// <param name="southLat">Южная широта</param>
        /// <param name="westLng">Западная долгота</param>
        /// <param name="northLat">Северная широта</param>
        /// <param name="eastLng">Восточная долгота</param>
        /// <returns>Список маркеров в указанном прямоугольнике</returns>
        Task<IEnumerable<MapMarker>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng);
        
        /// <summary>
        /// Получить маркеры в указанном регионе
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        /// <returns>Список маркеров в указанном регионе</returns>
        Task<IEnumerable<MapMarker>> GetByRegionIdAsync(int regionId);
        
        /// <summary>
        /// Создать маркер для растения
        /// </summary>
        /// <param name="specimen">Экземпляр растения</param>
        /// <returns>Созданный маркер</returns>
        Task<MapMarker> CreateMarkerForSpecimenAsync(Specimen specimen);
    }
} 