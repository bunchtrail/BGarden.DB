using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.DB.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация сущности MapLayer для EF Core
    /// </summary>
    public class MapLayerConfiguration : IEntityTypeConfiguration<MapLayer>
    {
        public void Configure(EntityTypeBuilder<MapLayer> builder)
        {
            builder.ToTable("MapLayers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.BaseDirectory)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.TileFormat)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("png");

            builder.Property(x => x.MinZoom)
                .HasDefaultValue(1);

            builder.Property(x => x.MaxZoom)
                .HasDefaultValue(18);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            
            // Конфигурация свойств для границ карты
            builder.Property(x => x.MinX)
                .HasColumnType("double precision");
                
            builder.Property(x => x.MinY)
                .HasColumnType("double precision");
                
            builder.Property(x => x.MaxX)
                .HasColumnType("double precision");
                
            builder.Property(x => x.MaxY)
                .HasColumnType("double precision");
            
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Индекс для быстрого поиска по имени
            builder.HasIndex(x => x.Name).IsUnique();

            // Индекс для фильтрации активных слоев
            builder.HasIndex(x => x.IsActive);
        }
    }
} 