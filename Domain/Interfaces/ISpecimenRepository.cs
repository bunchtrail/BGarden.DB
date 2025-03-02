using System.Threading.Tasks;
using BGarden.Domain.Entities;

namespace BGarden.Domain.Interfaces
{
    /// <summary>
    /// Специализированный репозиторий для работы с экземплярами растений (Specimen).
    /// </summary>
    public interface ISpecimenRepository : IRepository<Specimen>
    {
        Task<Specimen?> FindByInventoryNumberAsync(string inventoryNumber);
        Task<IEnumerable<Specimen>> FindBySpeciesNameAsync(string speciesName);
    }
} 