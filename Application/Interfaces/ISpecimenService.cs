using Application.DTO;
using BGarden.Domain.Enums;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы со Specimen (образцами).
    /// </summary>
    public interface ISpecimenService
    {
        /// <summary>
        /// Получить все Specimen в виде DTO.
        /// </summary>
        Task<IEnumerable<SpecimenDto>> GetAllSpecimensAsync();

        /// <summary>
        /// Получить один Specimen по идентификатору.
        /// </summary>
        Task<SpecimenDto?> GetSpecimenByIdAsync(int id);
        
        /// <summary>
        /// Получить отфильтрованные образцы по различным критериям
        /// </summary>
        /// <param name="name">Опциональный фильтр по имени</param>
        /// <param name="familyId">Опциональный фильтр по ID семейства</param>
        /// <param name="regionId">Опциональный фильтр по ID региона</param>
        /// <returns>Отфильтрованная коллекция образцов</returns>
        Task<IEnumerable<SpecimenDto>> GetFilteredSpecimensAsync(string? name = null, int? familyId = null, int? regionId = null);

        /// <summary>
        /// Создать новый Specimen (из DTO), вернёт созданный DTO (с Id).
        /// </summary>
        Task<SpecimenDto> CreateSpecimenAsync(SpecimenDto specimenDto);

        /// <summary>
        /// Обновить существующий Specimen, вернёт обновлённый DTO.
        /// </summary>
        Task<SpecimenDto?> UpdateSpecimenAsync(int id, SpecimenDto specimenDto);

        /// <summary>
        /// Удалить Specimen по Id, вернёт true, если успешно.
        /// </summary>
        Task<bool> DeleteSpecimenAsync(int id);

        /// <summary>
        /// Получить все образцы, принадлежащие указанному типу сектора.
        /// </summary>
        /// <param name="sectorType">Тип сектора (дендрология, флора, цветоводство)</param>
        /// <returns>Коллекция образцов, относящихся к указанному типу сектора</returns>
        Task<IEnumerable<SpecimenDto>> GetSpecimensBySectorTypeAsync(SectorType sectorType);
        
        /// <summary>
        /// Получить все образцы в пределах указанной области карты.
        /// </summary>
        /// <param name="minLat">Минимальная широта</param>
        /// <param name="minLng">Минимальная долгота</param>
        /// <param name="maxLat">Максимальная широта</param>
        /// <param name="maxLng">Максимальная долгота</param>
        /// <returns>Коллекция образцов, находящихся в указанной области</returns>
        Task<IEnumerable<SpecimenDto>> GetSpecimensInBoundingBoxAsync(decimal minLat, decimal minLng, decimal maxLat, decimal maxLng);
        
        /// <summary>
        /// Добавить новое растение на карту с указанными координатами.
        /// </summary>
        /// <param name="specimenDto">Данные о растении</param>
        /// <returns>Созданный экземпляр растения</returns>
        Task<SpecimenDto> AddSpecimenToMapAsync(SpecimenDto specimenDto);
        
        /// <summary>
        /// Обновить местоположение растения на карте.
        /// </summary>
        /// <param name="id">Идентификатор растения</param>
        /// <param name="latitude">Новая широта</param>
        /// <param name="longitude">Новая долгота</param>
        /// <returns>Обновленный экземпляр растения</returns>
        Task<SpecimenDto?> UpdateSpecimenLocationAsync(int id, decimal latitude, decimal longitude);
    }
} 