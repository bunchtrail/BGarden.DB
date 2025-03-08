using System;
using BGarden.DB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGarden.DB.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация области на карте для Entity Framework
    /// </summary>
    public class MapAreaConfiguration : IEntityTypeConfiguration<MapArea>
    {
        public void Configure(EntityTypeBuilder<MapArea> builder)
        {
            // Первичный ключ
            builder.HasKey(a => a.Id);

            // Основные свойства
            builder.Property(a => a.Name).IsRequired().HasMaxLength(150);
            builder.Property(a => a.Type).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.Color).HasMaxLength(50);
            builder.Property(a => a.FillColor).HasMaxLength(50);

            // Отношение с координатами области (один-ко-многим)
            builder.HasMany(a => a.Coordinates)
                   .WithOne(c => c.MapArea)
                   .HasForeignKey(c => c.MapAreaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Отношение с экспозицией (один-ко-многим)
            builder.HasOne(a => a.Exposition)
                   .WithMany()
                   .HasForeignKey(a => a.ExpositionId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Индекс для быстрого поиска по типу области
            builder.HasIndex(a => a.Type);

            // Дата создания и обновления
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();
        }
    }
} 