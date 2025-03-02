using System;
using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы с Biometry (биометрическими данными).
    /// </summary>
    public interface IBiometryService
    {
        /// <summary>
        /// Получить все биометрические данные в виде DTO.
        /// </summary>
        Task<IEnumerable<BiometryDto>> GetAllBiometriesAsync();

        /// <summary>
        /// Получить биометрические данные по идентификатору.
        /// </summary>
        Task<BiometryDto?> GetBiometryByIdAsync(int id);
        
        /// <summary>
        /// Получить биометрические данные по идентификатору образца.
        /// </summary>
        Task<IEnumerable<BiometryDto>> GetBiometriesBySpecimenIdAsync(int specimenId);
        
        /// <summary>
        /// Получить биометрические данные в указанном диапазоне дат.
        /// </summary>
        Task<IEnumerable<BiometryDto>> GetBiometriesByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Получить последние биометрические измерения для образца.
        /// </summary>
        Task<IEnumerable<BiometryDto>> GetLatestBiometriesForSpecimenAsync(int specimenId, int count = 1);

        /// <summary>
        /// Создать новую запись биометрических данных (из DTO), вернёт созданный DTO (с Id).
        /// </summary>
        Task<BiometryDto> CreateBiometryAsync(BiometryDto biometryDto);

        /// <summary>
        /// Обновить существующую запись биометрических данных, вернёт обновлённый DTO.
        /// </summary>
        Task<BiometryDto?> UpdateBiometryAsync(int id, BiometryDto biometryDto);

        /// <summary>
        /// Удалить запись биометрических данных по Id, вернёт true, если успешно.
        /// </summary>
        Task<bool> DeleteBiometryAsync(int id);
    }
} 