using System;
using BGarden.DB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGarden.DB.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация настроек карты для Entity Framework
    /// </summary>
    public class MapOptionsConfiguration : IEntityTypeConfiguration<MapOptions>
    {
        public void Configure(EntityTypeBuilder<MapOptions> builder)
        {
            // Первичный ключ
            builder.HasKey(o => o.Id);

            // Основные свойства
            builder.Property(o => o.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(o => o.CenterLatitude)
                  .IsRequired();

            builder.Property(o => o.CenterLongitude)
                  .IsRequired();

            builder.Property(o => o.DefaultZoom)
                  .IsRequired();

            builder.Property(o => o.MinZoom)
                  .IsRequired();

            builder.Property(o => o.MaxZoom)
                  .IsRequired();

            builder.Property(o => o.MapSchemaUrl)
                  .IsRequired()
                  .HasMaxLength(255);

            builder.Property(o => o.IsDefault)
                  .IsRequired()
                  .HasDefaultValue(false);

            // Дата создания и обновления
            builder.Property(o => o.CreatedAt)
                  .IsRequired();

            builder.Property(o => o.UpdatedAt)
                  .IsRequired();

            // Гарантия, что только одна конфигурация будет по умолчанию
            builder.HasIndex(o => o.IsDefault)
                  .HasFilter("\"IsDefault\" = TRUE")
                  .IsUnique();
        }
    }
} 