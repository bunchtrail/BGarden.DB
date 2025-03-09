using System.Threading.Tasks;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

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

        /// <summary>
        /// Получает все образцы, принадлежащие указанному типу сектора.
        /// </summary>
        /// <param name="sectorType">Тип сектора (дендрология, флора, цветоводство)</param>
        /// <returns>Коллекция образцов, относящихся к указанному типу сектора</returns>
        Task<IEnumerable<Specimen>> GetSpecimensBySectorTypeAsync(SectorType sectorType);

        /// <summary>
        /// Получает образец по идентификатору с включением связанных данных (семейство, экспозиция, биометрия, фенология).
        /// </summary>
        /// <param name="id">Идентификатор образца</param>
        /// <returns>Образец с загруженными связанными данными</returns>
        Task<Specimen?> GetByIdWithDetailsAsync(int id);
    }
} 