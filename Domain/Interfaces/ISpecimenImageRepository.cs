using Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для управления изображениями образцов
    /// </summary>
    public interface ISpecimenImageRepository
    {
        /// <summary>
        /// Получить все изображения для указанного образца
        /// </summary>
        /// <param name="specimenId">Идентификатор образца</param>
        /// <returns>Коллекция изображений</returns>
        Task<IEnumerable<SpecimenImage>> GetBySpecimenIdAsync(int specimenId);
        
        /// <summary>
        /// Получить основное изображение для указанного образца
        /// </summary>
        /// <param name="specimenId">Идентификатор образца</param>
        /// <returns>Основное изображение или null</returns>
        Task<SpecimenImage?> GetMainImageBySpecimenIdAsync(int specimenId);
        
        /// <summary>
        /// Получить изображение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <returns>Изображение или null</returns>
        Task<SpecimenImage?> GetByIdAsync(int id);
        
        /// <summary>
        /// Добавить новое изображение
        /// </summary>
        /// <param name="image">Объект изображения</param>
        /// <returns>Добавленное изображение</returns>
        Task<SpecimenImage> AddAsync(SpecimenImage image);
        
        /// <summary>
        /// Обновить существующее изображение
        /// </summary>
        /// <param name="image">Объект изображения с обновленными данными</param>
        /// <returns>Обновленное изображение</returns>
        Task<SpecimenImage> UpdateAsync(SpecimenImage image);
        
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