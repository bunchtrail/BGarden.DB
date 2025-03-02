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
    public class BiometryRepository : RepositoryBase<Biometry>, IBiometryRepository
    {
        public BiometryRepository(BotanicalContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<Biometry>> GetBySpecimenIdAsync(int specimenId)
        {
            return await _dbSet
                .Where(b => b.SpecimenId == specimenId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Biometry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(b => b.MeasurementDate >= startDate && b.MeasurementDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Biometry>> GetLatestForSpecimenAsync(int specimenId, int count = 1)
        {
            return await _dbSet
                .Where(b => b.SpecimenId == specimenId)
                .OrderByDescending(b => b.MeasurementDate)
                .Take(count)
                .ToListAsync();
        }

        public override async Task<Biometry?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Specimen)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
} 