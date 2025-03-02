using Microsoft.EntityFrameworkCore;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data
{
    public class BotanicalContext : DbContext
    {
        // Конструктор
        public BotanicalContext(DbContextOptions<BotanicalContext> options)
            : base(options)
        {
        }

        // Наборы сущностей
        public DbSet<Specimen> Specimens { get; set; } = null!;
        public DbSet<Family> Families { get; set; } = null!;
        public DbSet<Exposition> Expositions { get; set; } = null!;
        public DbSet<Phenology> Phenologies { get; set; } = null!;
        public DbSet<Biometry> Biometries { get; set; } = null!;

        // Переопределим OnModelCreating, чтобы применить конфигурации
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применяем конфигурации из папки Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BotanicalContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // Метод для настройки подключения к PostgreSQL
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString.PostgreSQL);
            }
        }
    }
} 