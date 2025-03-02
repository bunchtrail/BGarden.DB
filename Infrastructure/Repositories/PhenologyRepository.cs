using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class PhenologyRepository : RepositoryBase<Phenology>, IPhenologyRepository
    {
        public PhenologyRepository(BotanicalContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<Phenology>> GetBySpecimenIdAsync(int specimenId)
        {
            return await _dbSet
                .Where(p => p.SpecimenId == specimenId)
                .OrderByDescending(p => p.Year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Phenology>> GetByYearAsync(int year)
        {
            return await _dbSet
                .Where(p => p.Year == year)
                .ToListAsync();
        }

        public async Task<Phenology?> GetBySpecimenAndYearAsync(int specimenId, int year)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.SpecimenId == specimenId && p.Year == year);
        }

        public async Task<IEnumerable<Phenology>> GetByFloweringPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => 
                    (p.FloweringStart <= endDate && p.FloweringEnd >= startDate) || 
                    (p.FloweringStart >= startDate && p.FloweringStart <= endDate) ||
                    (p.FloweringEnd >= startDate && p.FloweringEnd <= endDate))
                .ToListAsync();
        }

        public override async Task<Phenology?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Specimen)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
} 