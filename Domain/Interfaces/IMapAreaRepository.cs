using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Enums;
using BGarden.Domain.Interfaces;

namespace BGarden.DB.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с областями на карте
    /// </summary>
    public interface IMapAreaRepository : IRepository<MapArea>
    {
        /// <summary>
        /// Получить области по типу
        /// </summary>
        /// <param name="type">Тип области</param>
        /// <returns>Список областей указанного типа</returns>
        Task<IEnumerable<MapArea>> GetByTypeAsync(AreaType type);

        /// <summary>
        /// Получить области, связанные с указанной экспозицией
        /// </summary>
        /// <param name="expositionId">Идентификатор экспозиции</param>
        /// <returns>Список областей, связанных с указанной экспозицией</returns>
        Task<IEnumerable<MapArea>> GetByExpositionIdAsync(int expositionId);

        /// <summary>
        /// Получить области, которые пересекаются с указанным географическим прямоугольником
        /// </summary>
        /// <param name="southLat">Южная широта</param>
        /// <param name="westLng">Западная долгота</param>
        /// <param name="northLat">Северная широта</param>
        /// <param name="eastLng">Восточная долгота</param>
        /// <returns>Список областей, пересекающихся с указанным прямоугольником</returns>
        Task<IEnumerable<MapArea>> GetInBoundsAsync(double southLat, double westLng, double northLat, double eastLng);
    }
} 