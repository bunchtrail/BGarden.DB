using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Базовая реализация репозитория для любой сущности T.
    /// </summary>
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly BotanicalContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(BotanicalContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
} 