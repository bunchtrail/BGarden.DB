using System.Threading.Tasks;
using System.Collections.Generic;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Специализированный репозиторий для работы с экспозициями (Exposition).
    /// </summary>
    public interface IExpositionRepository : IRepository<Exposition>
    {
        Task<Exposition?> FindByNameAsync(string name);
        Task<IEnumerable<Exposition>> GetWithSpecimensAsync();
    }
} 