using System.Threading.Tasks;
using System.Collections.Generic;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Специализированный репозиторий для работы с семействами растений (Family).
    /// </summary>
    public interface IFamilyRepository : IRepository<Family>
    {
        Task<Family?> FindByNameAsync(string name);
        Task<IEnumerable<Family>> GetWithSpecimensAsync();
    }
} 