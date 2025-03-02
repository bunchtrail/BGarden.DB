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

            // Настройка пространственного поля Location
            builder.Property(r => r.Location)
                   .HasColumnType("geography (point)");

            // Индекс для ускорения поиска по координатам
            builder.HasIndex(r => new { r.Latitude, r.Longitude })
                   .HasDatabaseName("IX_Region_Coordinates");

            // Пространственный индекс для поля Location
            builder.HasIndex(r => r.Location)
                   .HasMethod("SPATIAL")
                   .HasDatabaseName("IX_Region_Location_Spatial");

            // Необязательные поля
            builder.Property(r => r.Radius)
                   .HasColumnType("decimal(9,2)");

            builder.Property(r => r.BoundaryWkt)
                   .HasColumnType("nvarchar(max)");

            // Настройка пространственного поля Boundary
            builder.Property(r => r.Boundary)
                   .HasColumnType("geography (polygon)");

            // Связь с Specimen - одна область может содержать много растений
            builder.HasMany(r => r.Specimens)
                   .WithOne(s => s.Region)
                   .HasForeignKey(s => s.RegionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 