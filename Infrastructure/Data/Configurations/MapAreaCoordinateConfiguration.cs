using System;
using BGarden.DB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGarden.DB.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация координаты области на карте для Entity Framework
    /// </summary>
    public class MapAreaCoordinateConfiguration : IEntityTypeConfiguration<MapAreaCoordinate>
    {
        public void Configure(EntityTypeBuilder<MapAreaCoordinate> builder)
        {
            // Первичный ключ
            builder.HasKey(c => c.Id);

            // Основные свойства
            builder.Property(c => c.Latitude).IsRequired();
            builder.Property(c => c.Longitude).IsRequired();
            builder.Property(c => c.Order).IsRequired();

            // Отношение с областью (один-ко-многим)
            builder.HasOne(c => c.MapArea)
                   .WithMany(a => a.Coordinates)
                   .HasForeignKey(c => c.MapAreaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Индекс для быстрой сортировки координат по порядку
            builder.HasIndex(c => new { c.MapAreaId, c.Order });
        }
    }
} 