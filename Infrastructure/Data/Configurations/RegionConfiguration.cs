using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            // Название таблицы
            builder.ToTable("Regions");

            // Первичный ключ
            builder.HasKey(r => r.Id);

            // Настройка для поля SectorType - хранить как int
            builder.Property(r => r.SectorType)
                   .HasConversion<int>()
                   .IsRequired();

            // Обязательные поля и ограничения
            builder.Property(r => r.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(r => r.Description)
                   .HasMaxLength(1000);

            // Координаты центра области
            builder.Property(r => r.Latitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired();

            builder.Property(r => r.Longitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired();

            // Игнорируем пространственное поле Location в базе данных
            builder.Ignore(r => r.Location);

            // Добавляем поле для координат полигона
            builder.Property(r => r.PolygonCoordinates)
                   .HasColumnType("text");

            // Поля для стилизации полигона
            builder.Property(r => r.StrokeColor)
                   .HasMaxLength(50);

            builder.Property(r => r.FillColor)
                   .HasMaxLength(50);

            builder.Property(r => r.FillOpacity)
                   .HasColumnType("decimal(3,2)");

            // Индекс для ускорения поиска по координатам
            builder.HasIndex(r => new { r.Latitude, r.Longitude })
                   .HasDatabaseName("IX_Region_Coordinates");

            // Игнорируем поле Boundary в базе данных
            builder.Ignore(r => r.Boundary);

            // Даты создания и обновления
            builder.Property(r => r.CreatedAt)
                   .IsRequired();

            builder.Property(r => r.UpdatedAt)
                   .IsRequired();

            // Связь с Specimen - одна область может содержать много растений
            builder.HasMany(r => r.Specimens)
                   .WithOne(s => s.Region)
                   .HasForeignKey(s => s.RegionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 