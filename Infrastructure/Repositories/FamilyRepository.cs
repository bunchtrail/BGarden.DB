using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class FamilyRepository : RepositoryBase<Family>, IFamilyRepository
    {
        public FamilyRepository(BotanicalContext context) 
            : base(context)
        {
        }

        public async Task<Family?> FindByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Family>> GetWithSpecimensAsync()
        {
            return await _dbSet
                .Include(f => f.Specimens)
                .ToListAsync();
        }
    }
} 