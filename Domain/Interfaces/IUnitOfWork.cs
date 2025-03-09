using System;
using System.Threading.Tasks;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс Unit of Work. 
    /// Объединяет несколько операций репозиториев в одну транзакцию.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ISpecimenRepository Specimens { get; }
        IFamilyRepository Families { get; }
        IExpositionRepository Expositions { get; }
        IBiometryRepository Biometries { get; }
        IPhenologyRepository Phenologies { get; }
        IRegionRepository Regions { get; }
        IUserRepository Users { get; }
        IMapRepository Maps { get; }

        Task<int> SaveChangesAsync();
    }
} 