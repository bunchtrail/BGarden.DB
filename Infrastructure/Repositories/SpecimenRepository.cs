using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BGarden.Domain.Enums;
using NetTopologySuite.Geometries;

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
                .Where(x => (x.LatinName != null && x.LatinName.Contains(speciesName)) || 
                           (x.RussianName != null && x.RussianName.Contains(speciesName)))
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

        /// <inheritdoc/>
        public async Task<IEnumerable<Specimen>> GetSpecimensBySectorTypeAsync(SectorType sectorType)
        {
            return await _dbSet
                .Where(x => x.SectorType == sectorType)
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
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Specimen>> GetSpecimensInBoundingBoxAsync(Envelope boundingBox)
        {
            // Создаем полигон из границ области
            var polygon = new Polygon(new LinearRing(new Coordinate[] {
                new Coordinate(boundingBox.MinX, boundingBox.MinY),
                new Coordinate(boundingBox.MaxX, boundingBox.MinY),
                new Coordinate(boundingBox.MaxX, boundingBox.MaxY),
                new Coordinate(boundingBox.MinX, boundingBox.MaxY),
                new Coordinate(boundingBox.MinX, boundingBox.MinY)
            })) { SRID = 4326 };

            return await _dbSet
                .Where(s => s.Location != null && s.Location.Within(polygon))
                .Include(s => s.Family)
                .Include(s => s.Exposition)
                .ToListAsync();
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<Specimen>> GetFilteredSpecimensAsync(string? name = null, int? familyId = null, int? regionId = null)
        {
            // Начинаем с базового запроса
            var query = _dbSet.AsQueryable();
            
            // Добавляем фильтры, если они указаны
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s => 

                    (s.LatinName != null && s.LatinName.Contains(name)) ||
                    (s.RussianName != null && s.RussianName.Contains(name)));
            }
            
            if (familyId.HasValue)
            {
                query = query.Where(s => s.FamilyId == familyId.Value);
            }
            
            if (regionId.HasValue)
            {
                query = query.Where(s => s.RegionId == regionId.Value);
            }
            
            // Включаем связанные данные и выполняем запрос
            return await query
                .Include(s => s.Family)
                .Include(s => s.Exposition)
                .Include(s => s.Region)
                .ToListAsync();
        }
    }
} 