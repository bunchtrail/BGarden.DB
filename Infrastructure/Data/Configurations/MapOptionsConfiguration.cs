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
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
            builder.Property(o => o.CenterLatitude).IsRequired();
            builder.Property(o => o.CenterLongitude).IsRequired();
            builder.Property(o => o.Zoom).IsRequired();
            builder.Property(o => o.IsDefault).IsRequired().HasDefaultValue(false);

            // Индекс для быстрого поиска настроек по умолчанию
            builder.HasIndex(o => o.IsDefault);

            // Дата создания и обновления
            builder.Property(o => o.CreatedAt).IsRequired();
            builder.Property(o => o.UpdatedAt).IsRequired();
        }
    }
} 