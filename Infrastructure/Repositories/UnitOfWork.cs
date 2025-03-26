using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using Infrastructure.Repositories;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация Unit of Work, которая управляет транзакциями и репозиториями.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BotanicalContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;
        private IDbContextTransaction _transaction;
        
        // Используем static для хранения последнего созданного экземпляра UnitOfWork
        private static UnitOfWork _currentInstance;

        private ISpecimenRepository _specimenRepository;
        private IFamilyRepository _familyRepository;
        private IExpositionRepository _expositionRepository;
        private IBiometryRepository _biometryRepository;
        private IPhenologyRepository _phenologyRepository;
        private IRegionRepository _regionRepository;
        private IUserRepository _userRepository;
        private IMapRepository _mapRepository;
        private ISpecimenImageRepository _specimenImageRepository;

        public UnitOfWork(
            BotanicalContext context,
            ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
            
            // Сохраняем текущий экземпляр UnitOfWork
            _currentInstance = this;
        }
        
        /// <summary>
        /// Получает текущий экземпляр UnitOfWork
        /// </summary>
        public static IUnitOfWork GetUnitOfWork()
        {
            return _currentInstance;
        }

        public ISpecimenRepository Specimens => 
            _specimenRepository ??= new CachedSpecimenRepository(
                new SpecimenRepository(_context),
                new Infrastructure.Services.CacheService(
                    new Microsoft.Extensions.Caching.Memory.MemoryCache(
                        new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()),
                    new Microsoft.Extensions.Logging.Abstractions.NullLogger<Infrastructure.Services.CacheService>()),
                new Microsoft.Extensions.Logging.Abstractions.NullLogger<CachedSpecimenRepository>());

        public IFamilyRepository Families => 
            _familyRepository ??= new FamilyRepository(_context);

        public IExpositionRepository Expositions => 
            _expositionRepository ??= new ExpositionRepository(_context);

        public IBiometryRepository Biometries => 
            _biometryRepository ??= new BiometryRepository(_context);

        public IPhenologyRepository Phenologies => 
            _phenologyRepository ??= new PhenologyRepository(_context);

        public IRegionRepository Regions => 
            _regionRepository ??= new RegionRepository(_context);

        public IUserRepository Users => 
            _userRepository ??= new UserRepository(_context);
            
        public IMapRepository Maps => 
            _mapRepository ??= new MapRepository(_context);
            
        public ISpecimenImageRepository SpecimenImages => 
            _specimenImageRepository ??= new SpecimenImageRepository(
                _context,
                new Microsoft.Extensions.Logging.Abstractions.NullLogger<SpecimenImageRepository>());

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
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