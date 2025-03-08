using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Enums;
using BGarden.Domain.Interfaces;

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
        /// Получить маркеры, связанные с указанным экземпляром растения
        /// </summary>
        /// <param name="specimenId">Идентификатор экземпляра растения</param>
        /// <returns>Список маркеров, связанных с указанным экземпляром</returns>
        Task<IEnumerable<MapMarker>> GetBySpecimenIdAsync(int specimenId);

        /// <summary>
        /// Получить маркеры в указанном географическом прямоугольнике
        /// </summary>
        /// <param name="southLat">Южная широта</param>
        /// <param name="westLng">Западная долгота</param>
        /// <param name="northLat">Северная широта</param>
        /// <param name="eastLng">Восточная долгота</param>
        /// <returns>Список маркеров в указанном прямоугольнике</returns>
        Task<IEnumerable<MapMarker>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng);
    }
} 