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
        
        /// <summary>
        /// Получает все образцы, принадлежащие указанному региону (области).
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        /// <returns>Коллекция образцов, расположенных в данном регионе</returns>
        Task<IEnumerable<Specimen>> GetSpecimensByRegionIdAsync(int regionId);
    }
} 