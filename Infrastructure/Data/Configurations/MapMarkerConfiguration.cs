using System;
using BGarden.DB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            builder.Property(m => m.PopupContent).HasMaxLength(2000);
            
            // Отношение с экземпляром растения (один-ко-многим)
            builder.HasOne(m => m.Specimen)
                   .WithMany()
                   .HasForeignKey(m => m.SpecimenId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Дата создания и обновления
            builder.Property(m => m.CreatedAt).IsRequired();
            builder.Property(m => m.UpdatedAt).IsRequired();
        }
    }
} 