using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы с Exposition (экспозициями).
    /// </summary>
    public interface IExpositionService
    {
        /// <summary>
        /// Получить все экспозиции в виде DTO.
        /// </summary>
        Task<IEnumerable<ExpositionDto>> GetAllExpositionsAsync();

        /// <summary>
        /// Получить экспозицию по идентификатору.
        /// </summary>
        Task<ExpositionDto?> GetExpositionByIdAsync(int id);
        
        /// <summary>
        /// Получить экспозицию по названию.
        /// </summary>
        Task<ExpositionDto?> GetExpositionByNameAsync(string name);

        /// <summary>
        /// Создать новую экспозицию (из DTO), вернёт созданный DTO (с Id).
        /// </summary>
        Task<ExpositionDto> CreateExpositionAsync(ExpositionDto expositionDto);

        /// <summary>
        /// Обновить существующую экспозицию, вернёт обновлённый DTO.
        /// </summary>
        Task<ExpositionDto?> UpdateExpositionAsync(int id, ExpositionDto expositionDto);

        /// <summary>
        /// Удалить экспозицию по Id, вернёт true, если успешно.
        /// </summary>
        Task<bool> DeleteExpositionAsync(int id);
    }
} 