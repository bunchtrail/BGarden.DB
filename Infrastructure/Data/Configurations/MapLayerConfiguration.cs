using System;
using BGarden.DB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGarden.DB.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация слоя карты для Entity Framework
    /// </summary>
    public class MapLayerConfiguration : IEntityTypeConfiguration<MapLayer>
    {
        public void Configure(EntityTypeBuilder<MapLayer> builder)
        {
            // Первичный ключ
            builder.HasKey(l => l.Id);

            // Основные свойства
            builder.Property(l => l.LayerId).IsRequired().HasMaxLength(50);
            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Url).IsRequired().HasMaxLength(500);
            builder.Property(l => l.Attribution).IsRequired().HasMaxLength(300);
            builder.Property(l => l.Description).HasMaxLength(500);
            builder.Property(l => l.IsDefault).IsRequired().HasDefaultValue(false);
            builder.Property(l => l.DisplayOrder).IsRequired().HasDefaultValue(0);

            // Уникальный индекс для LayerId
            builder.HasIndex(l => l.LayerId).IsUnique();

            // Индекс для быстрого поиска слоев по умолчанию
            builder.HasIndex(l => l.IsDefault);

            // Индекс для сортировки слоев по порядку отображения
            builder.HasIndex(l => l.DisplayOrder);

            // Дата создания и обновления
            builder.Property(l => l.CreatedAt).IsRequired();
            builder.Property(l => l.UpdatedAt).IsRequired();
        }
    }
} 