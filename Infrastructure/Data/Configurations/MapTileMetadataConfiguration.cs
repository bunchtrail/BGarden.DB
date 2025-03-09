using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.DB.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация сущности MapTileMetadata для EF Core
    /// </summary>
    public class MapTileMetadataConfiguration : IEntityTypeConfiguration<MapTileMetadata>
    {
        public void Configure(EntityTypeBuilder<MapTileMetadata> builder)
        {
            builder.ToTable("MapTileMetadata");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ZoomLevel)
                .IsRequired();

            builder.Property(x => x.TileColumn)
                .IsRequired();

            builder.Property(x => x.TileRow)
                .IsRequired();

            builder.Property(x => x.FileSize)
                .IsRequired();

            builder.Property(x => x.Checksum)
                .HasMaxLength(64);

            builder.Property(x => x.RelativePath)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Уникальный индекс для поиска тайла по координатам
            builder.HasIndex(x => new { x.MapLayerId, x.ZoomLevel, x.TileColumn, x.TileRow })
                .IsUnique();

            // Отношение с MapLayer
            builder.HasOne(x => x.MapLayer)
                .WithMany(x => x.TileMetadata)
                .HasForeignKey(x => x.MapLayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 