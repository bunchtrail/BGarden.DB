using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.Domain.Interfaces;

namespace BGarden.DB.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с настройками карты
    /// </summary>
    public interface IMapOptionsRepository : IRepository<MapOptions>
    {
        /// <summary>
        /// Получить все настройки карты
        /// </summary>
        Task<IEnumerable<MapOptions>> GetAllAsync();
        
        /// <summary>
        /// Получить настройки карты по идентификатору
        /// </summary>
        Task<MapOptions> GetByIdAsync(int id);
        
        /// <summary>
        /// Получить настройки карты по умолчанию
        /// </summary>
        /// <returns>Настройки карты по умолчанию или null, если таковых нет</returns>
        Task<MapOptions> GetDefaultAsync();
        
        /// <summary>
        /// Добавить новые настройки карты
        /// </summary>
        Task AddAsync(MapOptions options);
        
        /// <summary>
        /// Обновить существующие настройки карты
        /// </summary>
        void Update(MapOptions options);
        
        /// <summary>
        /// Удалить настройки карты
        /// </summary>
        void Remove(MapOptions options);

        /// <summary>
        /// Установить настройки карты по умолчанию
        /// </summary>
        /// <param name="id">Идентификатор настроек, которые нужно сделать используемыми по умолчанию</param>
        /// <returns>Обновленные настройки карты</returns>
        Task<MapOptions> SetDefaultAsync(int id);
    }
} 