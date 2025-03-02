using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Базовый интерфейс репозитория для сущности T.
    /// </summary>
    /// <typeparam name="T">Доменная сущность</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
} 