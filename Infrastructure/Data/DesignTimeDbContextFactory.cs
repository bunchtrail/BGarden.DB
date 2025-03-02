using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BGarden.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BotanicalContext>
    {
        public BotanicalContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BotanicalContext>();
            optionsBuilder.UseNpgsql(ConnectionString.PostgreSQL);

            return new BotanicalContext(optionsBuilder.Options);
        }
    }
} 