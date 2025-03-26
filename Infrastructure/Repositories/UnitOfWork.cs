using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BotanicalContext _context;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IExpositionRepository _expositionRepository;
        private readonly IBiometryRepository _biometryRepository;
        private readonly IPhenologyRepository _phenologyRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapRepository _mapRepository;
        private readonly ISpecimenImageRepository _specimenImageRepository;
        private bool _disposed = false;
        private IDbContextTransaction _transaction;

        public UnitOfWork(
            BotanicalContext context,
            ISpecimenRepository specimenRepository,
            IFamilyRepository familyRepository,
            IExpositionRepository expositionRepository,
            IBiometryRepository biometryRepository,
            IPhenologyRepository phenologyRepository,
            IRegionRepository regionRepository,
            IUserRepository userRepository,
            IMapRepository mapRepository,
            ISpecimenImageRepository specimenImageRepository)
        {
            _context = context;
            _specimenRepository = specimenRepository;
            _familyRepository = familyRepository;
            _expositionRepository = expositionRepository;
            _biometryRepository = biometryRepository;
            _phenologyRepository = phenologyRepository;
            _regionRepository = regionRepository;
            _userRepository = userRepository;
            _mapRepository = mapRepository;
            _specimenImageRepository = specimenImageRepository;
        }

        public ISpecimenRepository Specimens => _specimenRepository;
        public IFamilyRepository Families => _familyRepository;
        public IExpositionRepository Expositions => _expositionRepository;
        public IBiometryRepository Biometries => _biometryRepository;
        public IPhenologyRepository Phenologies => _phenologyRepository;
        public IRegionRepository Regions => _regionRepository;
        public IUserRepository Users => _userRepository;
        public IMapRepository Maps => _mapRepository;
        public ISpecimenImageRepository SpecimenImages => _specimenImageRepository;

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