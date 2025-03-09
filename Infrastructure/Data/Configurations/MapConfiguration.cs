using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация для сущности Map
    /// </summary>
    public class MapConfiguration : IEntityTypeConfiguration<Map>
    {
        public void Configure(EntityTypeBuilder<Map> builder)
        {
            builder.ToTable("Maps");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).UseIdentityColumn();

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            builder.Property(m => m.FilePath)
                .HasMaxLength(255);

            builder.Property(m => m.ContentType)
                .HasMaxLength(50);

            builder.Property(m => m.FileSize);

            builder.Property(m => m.UploadDate)
                .IsRequired();

            builder.Property(m => m.LastUpdated)
                .IsRequired();

            builder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Индексы
            builder.HasIndex(m => m.Name).IsUnique();
            builder.HasIndex(m => m.IsActive);
            
            // Связи
            builder.HasMany(m => m.Specimens)
                .WithOne(s => s.Map)
                .HasForeignKey(s => s.MapId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 