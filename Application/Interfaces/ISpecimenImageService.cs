using Application.DTO;

namespace Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления изображениями образцов
    /// </summary>
    public interface ISpecimenImageService
    {
        /// <summary>
        /// Получить все изображения для указанного образца
        /// </summary>
        /// <param name="specimenId">Идентификатор образца</param>
        /// <param name="includeImageData">Включать ли данные изображения</param>
        /// <returns>Коллекция DTO изображений</returns>
        Task<IEnumerable<SpecimenImageDto>> GetBySpecimenIdAsync(int specimenId, bool includeImageData = false);
        
        /// <summary>
        /// Получить основное изображение для указанного образца
        /// </summary>
        /// <param name="specimenId">Идентификатор образца</param>
        /// <param name="includeImageData">Включать ли данные изображения</param>
        /// <returns>DTO основного изображения или null</returns>
        Task<SpecimenImageDto?> GetMainImageBySpecimenIdAsync(int specimenId, bool includeImageData = true);
        
        /// <summary>
        /// Получить изображение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <param name="includeImageData">Включать ли данные изображения</param>
        /// <returns>DTO изображения или null</returns>
        Task<SpecimenImageDto?> GetByIdAsync(int id, bool includeImageData = true);
        
        /// <summary>
        /// Добавить новое изображение
        /// </summary>
        /// <param name="dto">DTO создания изображения</param>
        /// <returns>DTO добавленного изображения</returns>
        Task<SpecimenImageDto> AddAsync(CreateSpecimenImageDto dto);
        
        /// <summary>
        /// Добавить новое изображение с бинарными данными
        /// </summary>
        /// <param name="dto">DTO создания изображения с бинарными данными</param>
        /// <returns>DTO добавленного изображения</returns>
        Task<SpecimenImageDto> AddBinaryAsync(CreateSpecimenImageBinaryDto dto);
        
        /// <summary>
        /// Добавить несколько изображений для образца
        /// </summary>
        /// <param name="dtos">Коллекция DTO создания изображений</param>
        /// <returns>Коллекция DTO добавленных изображений</returns>
        Task<IEnumerable<SpecimenImageDto>> AddMultipleAsync(IEnumerable<CreateSpecimenImageBinaryDto> dtos);
        
        /// <summary>
        /// Обновить существующее изображение
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <param name="dto">DTO обновления изображения</param>
        /// <returns>DTO обновленного изображения</returns>
        Task<SpecimenImageDto?> UpdateAsync(int id, UpdateSpecimenImageDto dto);
        
        /// <summary>
        /// Удалить изображение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <returns>True, если удаление выполнено успешно</returns>
        Task<bool> DeleteAsync(int id);
        
        /// <summary>
        /// Установить указанное изображение как основное для образца
        /// </summary>
        /// <param name="imageId">Идентификатор изображения</param>
        /// <returns>True, если операция выполнена успешно</returns>
        Task<bool> SetAsMainAsync(int imageId);
    }
} 