using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;
using BGarden.Domain.Enums;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы с регионами (областями) ботанического сада
    /// </summary>
    public interface IRegionService
    {
        /// <summary>
        /// Получить все регионы
        /// </summary>
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync();

        /// <summary>
        /// Получить регион по идентификатору
        /// </summary>
        Task<RegionDto?> GetRegionByIdAsync(int id);

        /// <summary>
        /// Получить все регионы с определенным типом сектора
        /// </summary>
        Task<IEnumerable<RegionDto>> GetRegionsBySectorTypeAsync(SectorType sectorType);

        /// <summary>
        /// Получить регионы с растениями внутри
        /// </summary>
        Task<IEnumerable<RegionDto>> GetRegionsWithSpecimensAsync();

        /// <summary>
        /// Найти ближайшие регионы к заданным координатам в пределах указанного радиуса (в метрах)
        /// </summary>
        Task<IEnumerable<RegionDto>> FindNearbyRegionsAsync(decimal latitude, decimal longitude, decimal radiusInMeters);

        /// <summary>
        /// Создать новый регион (из DTO), вернёт созданный DTO (с Id)
        /// </summary>
        Task<RegionDto> CreateRegionAsync(RegionDto regionDto);

        /// <summary>
        /// Обновить существующий регион, вернёт обновлённый DTO
        /// </summary>
        Task<RegionDto?> UpdateRegionAsync(int id, RegionDto regionDto);

        /// <summary>
        /// Удалить регион по Id, вернёт true, если успешно
        /// </summary>
        Task<bool> DeleteRegionAsync(int id);

        /// <summary>
        /// Получить все растения (экземпляры) в заданном регионе
        /// </summary>
        Task<IEnumerable<SpecimenDto>> GetSpecimensInRegionAsync(int regionId);
    }
} 