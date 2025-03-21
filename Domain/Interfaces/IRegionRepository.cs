using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с областями (зонами, оранжереями) сада
    /// </summary>
    public interface IRegionRepository : IRepository<Region>
    {
        /// <summary>
        /// Получить все области
        /// </summary>
        Task<IEnumerable<Region>> GetAllRegionsAsync();

        /// <summary>
        /// Получить область по идентификатору
        /// </summary>
        Task<Region?> GetRegionByIdAsync(int id);

        /// <summary>
        /// Получить все области с определенным типом сектора
        /// </summary>
        Task<IEnumerable<Region>> GetRegionsBySectorTypeAsync(BGarden.Domain.Enums.SectorType sectorType);

        /// <summary>
        /// Получить области с растениями внутри
        /// </summary>
        Task<IEnumerable<Region>> GetRegionsWithSpecimensAsync();

        /// <summary>
        /// Найти ближайшие области к заданным координатам в пределах указанного радиуса (в метрах)
        /// </summary>
        Task<IEnumerable<Region>> FindNearbyRegionsAsync(decimal latitude, decimal longitude, decimal radiusInMeters);

        /// <summary>
        /// Проверить, находится ли точка внутри полигона области
        /// </summary>
        Task<bool> IsPointInRegionAsync(int regionId, decimal latitude, decimal longitude);

        /// <summary>
        /// Получить все растения, находящиеся в указанной области
        /// </summary>
        Task<IEnumerable<Specimen>> GetSpecimensInRegionAsync(int regionId);
        
        /// <summary>
        /// Обновить полигон области
        /// </summary>
        Task UpdateRegionPolygonAsync(int regionId, string polygonCoordinates, string? strokeColor = null, string? fillColor = null, decimal? fillOpacity = null);
    }
} 