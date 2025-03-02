using Application.DTO;

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
    }
} 