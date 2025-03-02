using System;
using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Контракт прикладного сервиса для работы с Phenology (фенологическими данными).
    /// </summary>
    public interface IPhenologyService
    {
        /// <summary>
        /// Получить все фенологические данные в виде DTO.
        /// </summary>
        Task<IEnumerable<PhenologyDto>> GetAllPhenologiesAsync();

        /// <summary>
        /// Получить фенологические данные по идентификатору.
        /// </summary>
        Task<PhenologyDto?> GetPhenologyByIdAsync(int id);
        
        /// <summary>
        /// Получить фенологические данные по идентификатору образца.
        /// </summary>
        Task<IEnumerable<PhenologyDto>> GetPhenologiesBySpecimenIdAsync(int specimenId);
        
        /// <summary>
        /// Получить фенологические данные за указанный год.
        /// </summary>
        Task<IEnumerable<PhenologyDto>> GetPhenologiesByYearAsync(int year);
        
        /// <summary>
        /// Получить фенологические данные конкретного образца за указанный год.
        /// </summary>
        Task<PhenologyDto?> GetPhenologyBySpecimenAndYearAsync(int specimenId, int year);
        
        /// <summary>
        /// Получить фенологические данные с цветением в указанный период.
        /// </summary>
        Task<IEnumerable<PhenologyDto>> GetPhenologiesByFloweringPeriodAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Создать новую запись фенологических данных (из DTO), вернёт созданный DTO (с Id).
        /// </summary>
        Task<PhenologyDto> CreatePhenologyAsync(PhenologyDto phenologyDto);

        /// <summary>
        /// Обновить существующую запись фенологических данных, вернёт обновлённый DTO.
        /// </summary>
        Task<PhenologyDto?> UpdatePhenologyAsync(int id, PhenologyDto phenologyDto);

        /// <summary>
        /// Удалить запись фенологических данных по Id, вернёт true, если успешно.
        /// </summary>
        Task<bool> DeletePhenologyAsync(int id);
    }
} 