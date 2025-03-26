using System;
using System.Threading.Tasks;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса кэширования для оптимизации запросов к базе данных
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Получить значение из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого объекта</typeparam>
        /// <param name="key">Ключ кэша</param>
        /// <returns>Закэшированный объект или default(T) если объект не найден</returns>
        Task<T?> GetAsync<T>(string key) where T : class;
        
        /// <summary>
        /// Сохранить значение в кэш с указанным временем жизни
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого объекта</typeparam>
        /// <param name="key">Ключ кэша</param>
        /// <param name="value">Значение для кэширования</param>
        /// <param name="expirationTime">Время жизни кэша</param>
        /// <returns>True если успешно сохранено, False в случае ошибки</returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expirationTime) where T : class;
        
        /// <summary>
        /// Удалить значение из кэша по ключу
        /// </summary>
        /// <param name="key">Ключ кэша</param>
        /// <returns>True если успешно удалено, False в случае ошибки</returns>
        Task<bool> RemoveAsync(string key);
        
        /// <summary>
        /// Получить или создать значение в кэше. Если значение в кэше отсутствует, 
        /// выполняется фабричный метод для создания значения, которое затем кэшируется
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого объекта</typeparam>
        /// <param name="key">Ключ кэша</param>
        /// <param name="factory">Функция для создания значения, если оно отсутствует в кэше</param>
        /// <param name="expirationTime">Время жизни кэша</param>
        /// <returns>Значение из кэша или новое созданное значение</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expirationTime) where T : class;
        
        /// <summary>
        /// Удаляет все кэши, ключи которых содержат указанный шаблон
        /// </summary>
        /// <param name="pattern">Шаблон для поиска ключей (подстрока)</param>
        /// <returns>Количество удаленных ключей</returns>
        Task<int> RemoveByPatternAsync(string pattern);
    }
} 