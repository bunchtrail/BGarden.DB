using System;
using System.Threading;
using System.Threading.Tasks;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BGarden.Infrastructure.Services
{
    /// <summary>
    /// Реализация сервиса кэширования на основе MemoryCache
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_memoryCache.TryGetValue(key, out T? value))
                {
                    _logger.LogDebug("Значение получено из кэша по ключу: {Key}", key);
                    return Task.FromResult(value);
                }
                
                _logger.LogDebug("Значение не найдено в кэше для ключа: {Key}", key);
                return Task.FromResult<T?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении значения из кэша для ключа: {Key}", key);
                return Task.FromResult<T?>(null);
            }
        }

        /// <inheritdoc/>
        public Task<bool> SetAsync<T>(string key, T value, TimeSpan expirationTime) where T : class
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expirationTime)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(key, value, cacheEntryOptions);
                _logger.LogDebug("Значение сохранено в кэш по ключу: {Key} с временем жизни: {ExpirationTime}", key, expirationTime);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении значения в кэш для ключа: {Key}", key);
                return Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                _logger.LogDebug("Значение удалено из кэша по ключу: {Key}", key);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении значения из кэша для ключа: {Key}", key);
                return Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expirationTime) where T : class
        {
            // Проверяем, есть ли значение в кэше
            T? result = await GetAsync<T>(key);
            if (result != null)
            {
                return result;
            }

            // Если значения нет в кэше, используем семафор для предотвращения параллельного создания
            try
            {
                await _semaphore.WaitAsync();
                
                // Проверяем еще раз, возможно значение уже было создано другим потоком
                result = await GetAsync<T>(key);
                if (result != null)
                {
                    return result;
                }

                // Создаем новое значение
                result = await factory();
                
                // Сохраняем значение в кэш
                await SetAsync(key, result, expirationTime);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении или создании значения в кэше для ключа: {Key}", key);
                // В случае ошибки создаем значение без кэширования
                return await factory();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
} 