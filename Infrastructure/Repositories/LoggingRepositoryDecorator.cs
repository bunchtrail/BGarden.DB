using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Базовый класс-декоратор для логирования операций репозитория
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class LoggingRepositoryDecorator<T> : IRepository<T> where T : class
    {
        protected readonly IRepository<T> _repository;
        protected readonly ILogger _logger;

        protected LoggingRepositoryDecorator(IRepository<T> repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try 
            {
                _logger.LogDebug("Выполняется GetByIdAsync для {EntityType} с id={Id}", typeof(T).Name, id);
                var result = await _repository.GetByIdAsync(id);
                stopwatch.Stop();
                _logger.LogDebug("GetByIdAsync для {EntityType} с id={Id} выполнено за {ElapsedMilliseconds}мс. Результат: {Result}", 
                    typeof(T).Name, id, stopwatch.ElapsedMilliseconds, result != null ? "найдено" : "не найдено");
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Ошибка в GetByIdAsync для {EntityType} с id={Id}. Время выполнения: {ElapsedMilliseconds}мс", 
                    typeof(T).Name, id, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try 
            {
                _logger.LogDebug("Выполняется GetAllAsync для {EntityType}", typeof(T).Name);
                var result = await _repository.GetAllAsync();
                stopwatch.Stop();
                _logger.LogDebug("GetAllAsync для {EntityType} выполнено за {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Ошибка в GetAllAsync для {EntityType}. Время выполнения: {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try 
            {
                _logger.LogDebug("Выполняется AddAsync для {EntityType}", typeof(T).Name);
                await _repository.AddAsync(entity);
                stopwatch.Stop();
                _logger.LogDebug("AddAsync для {EntityType} выполнено за {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Ошибка в AddAsync для {EntityType}. Время выполнения: {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try 
            {
                _logger.LogDebug("Выполняется Update для {EntityType}", typeof(T).Name);
                _repository.Update(entity);
                stopwatch.Stop();
                _logger.LogDebug("Update для {EntityType} выполнено за {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Ошибка в Update для {EntityType}. Время выполнения: {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public virtual void Remove(T entity)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try 
            {
                _logger.LogDebug("Выполняется Remove для {EntityType}", typeof(T).Name);
                _repository.Remove(entity);
                stopwatch.Stop();
                _logger.LogDebug("Remove для {EntityType} выполнено за {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Ошибка в Remove для {EntityType}. Время выполнения: {ElapsedMilliseconds}мс", 
                    typeof(T).Name, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
} 