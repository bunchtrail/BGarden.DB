using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;

namespace BGarden.Infrastructure.Repositories
{
    public class ExpositionRepository : RepositoryBase<Exposition>, IExpositionRepository
    {
        public ExpositionRepository(BotanicalContext context) 
            : base(context)
        {
        }

        public async Task<Exposition?> FindByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Exposition>> GetWithSpecimensAsync()
        {
            return await _dbSet
                .Include(e => e.Specimens)
                .ToListAsync();
        }
    }
} 