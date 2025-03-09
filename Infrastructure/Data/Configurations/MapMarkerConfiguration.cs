using System;
using BGarden.DB.Domain.Entities;
using BGarden.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;

namespace BGarden.DB.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация маркера на карте для Entity Framework
    /// </summary>
    public class MapMarkerConfiguration : IEntityTypeConfiguration<MapMarker>
    {
        public void Configure(EntityTypeBuilder<MapMarker> builder)
        {
            // Первичный ключ
            builder.HasKey(m => m.Id);

            // Основные свойства
            builder.Property(m => m.Title).IsRequired().HasMaxLength(150);
            builder.Property(m => m.Type).IsRequired();
            builder.Property(m => m.Latitude).IsRequired();
            builder.Property(m => m.Longitude).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(500);
            
            // Настройка пространственного поля Location
            builder.Property(m => m.Location)
                   .HasColumnType("geometry(Point,4326)");
            
            // Создаем пространственный индекс для Location
            builder.HasIndex(m => m.Location)
                   .HasDatabaseName("IX_MapMarker_Location_Spatial");
            
            // Отношение с экземпляром растения (один-к-одному)
            builder.HasOne(m => m.Specimen)
                   .WithOne(s => s.MapMarker)
                   .HasForeignKey<MapMarker>(m => m.SpecimenId)
                   .OnDelete(DeleteBehavior.Cascade);
                   
            // Отношение с регионом (много-к-одному)
            builder.HasOne<Region>()
                   .WithMany()
                   .HasForeignKey(m => m.RegionId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Дата создания и обновления
            builder.Property(m => m.CreatedAt).IsRequired();
            builder.Property(m => m.UpdatedAt).IsRequired();
        }
    }
} 