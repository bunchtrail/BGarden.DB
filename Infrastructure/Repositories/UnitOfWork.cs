using System;
using System.Threading.Tasks;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BotanicalContext _context;
        private readonly ISpecimenRepository _specimenRepository;
        private bool _disposed = false;

        public UnitOfWork(
            BotanicalContext context,
            ISpecimenRepository specimenRepository)
        {
            _context = context;
            _specimenRepository = specimenRepository;
        }

        public ISpecimenRepository Specimens => _specimenRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 