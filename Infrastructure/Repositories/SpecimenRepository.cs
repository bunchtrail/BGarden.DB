using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class SpecimenRepository : RepositoryBase<Specimen>, ISpecimenRepository
    {
        public SpecimenRepository(BotanicalContext context) 
            : base(context)
        {
        }

        public async Task<Specimen?> FindByInventoryNumberAsync(string inventoryNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.InventoryNumber == inventoryNumber);
        }

        public async Task<IEnumerable<Specimen>> FindBySpeciesNameAsync(string speciesName)
        {
            return await _dbSet
                .Where(x => x.LatinName.Contains(speciesName) || x.RussianName.Contains(speciesName))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Specimen>> GetSpecimensByRegionIdAsync(int regionId)
        {
            return await _dbSet
                .Where(x => x.RegionId == regionId)
                .Include(s => s.Family)
                .Include(s => s.Exposition)
                .ToListAsync();
        }

        // Можно добавить дополнительные методы для загрузки связанных данных, например:
        public async Task<Specimen?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Family)
                .Include(s => s.Exposition)
                .Include(s => s.Biometries)
                .Include(s => s.Phenologies)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
} 