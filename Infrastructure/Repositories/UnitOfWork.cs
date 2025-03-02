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
        private readonly IFamilyRepository _familyRepository;
        private readonly IExpositionRepository _expositionRepository;
        private readonly IBiometryRepository _biometryRepository;
        private readonly IPhenologyRepository _phenologyRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IUserRepository _userRepository;
        private bool _disposed = false;

        public UnitOfWork(
            BotanicalContext context,
            ISpecimenRepository specimenRepository,
            IFamilyRepository familyRepository,
            IExpositionRepository expositionRepository,
            IBiometryRepository biometryRepository,
            IPhenologyRepository phenologyRepository,
            IRegionRepository regionRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _specimenRepository = specimenRepository;
            _familyRepository = familyRepository;
            _expositionRepository = expositionRepository;
            _biometryRepository = biometryRepository;
            _phenologyRepository = phenologyRepository;
            _regionRepository = regionRepository;
            _userRepository = userRepository;
        }

        public ISpecimenRepository Specimens => _specimenRepository;
        public IFamilyRepository Families => _familyRepository;
        public IExpositionRepository Expositions => _expositionRepository;
        public IBiometryRepository Biometries => _biometryRepository;
        public IPhenologyRepository Phenologies => _phenologyRepository;
        public IRegionRepository Regions => _regionRepository;
        public IUserRepository Users => _userRepository;

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