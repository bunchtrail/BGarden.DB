using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы с Family (семействами).
    /// </summary>
    public interface IFamilyService
    {
        /// <summary>
        /// Получить все семейства в виде DTO.
        /// </summary>
        Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync();

        /// <summary>
        /// Получить семейство по идентификатору.
        /// </summary>
        Task<FamilyDto?> GetFamilyByIdAsync(int id);
        
        /// <summary>
        /// Получить семейство по названию.
        /// </summary>
        Task<FamilyDto?> GetFamilyByNameAsync(string name);

        /// <summary>
        /// Создать новое семейство (из DTO), вернёт созданный DTO (с Id).
        /// </summary>
        Task<FamilyDto> CreateFamilyAsync(FamilyDto familyDto);

        /// <summary>
        /// Обновить существующее семейство, вернёт обновлённый DTO.
        /// </summary>
        Task<FamilyDto?> UpdateFamilyAsync(int id, FamilyDto familyDto);

        /// <summary>
        /// Удалить семейство по Id, вернёт true, если успешно.
        /// </summary>
        Task<bool> DeleteFamilyAsync(int id);
    }
} 