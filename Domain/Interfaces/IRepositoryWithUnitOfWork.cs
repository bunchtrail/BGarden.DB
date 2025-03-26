using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Расширенный интерфейс репозитория с доступом к UnitOfWork
    /// </summary>
    /// <typeparam name="T">Доменная сущность</typeparam>
    public interface IRepositoryWithUnitOfWork<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Доступ к UnitOfWork для выполнения операций сохранения
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
} 