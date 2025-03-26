using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using NetTopologySuite.Geometries;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class SpecimenConfiguration : IEntityTypeConfiguration<Specimen>
    {
        public void Configure(EntityTypeBuilder<Specimen> builder)
        {
            // Название таблицы (если хотите другое, нежели "Specimens")
            // builder.ToTable("Specimens");

            // Первичный ключ
            builder.HasKey(s => s.Id);

            // Настройка для поля SectorType - хранить как int
            builder.Property(s => s.SectorType)
                   .HasConversion<int>()
                   .IsRequired();

            // Координаты местоположения растения
            builder.Property(s => s.Latitude)
                   .HasColumnType("decimal(9,6)");

            builder.Property(s => s.Longitude)
                   .HasColumnType("decimal(9,6)");

            // Настраиваем пространственное поле Location в базе данных
            builder.Property(s => s.Location)
                   .HasColumnType("geometry(Point,4326)");

            // Индекс для ускорения поиска по координатам (без фильтра, который создает проблемы)
            builder.HasIndex(s => new { s.Latitude, s.Longitude })
                   .HasDatabaseName("IX_Specimen_Coordinates");
            
            // Добавляем пространственный индекс для поля Location
            builder.HasIndex(s => s.Location)
                   .HasDatabaseName("IX_Specimen_Location_Spatial");

            // Связь с Region (Many-to-One)
            builder.HasOne(s => s.Region)
                   .WithMany(r => r.Specimens)
                   .HasForeignKey(s => s.RegionId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Уникальный индекс на InventoryNumber, если требуется
            builder.HasIndex(s => s.InventoryNumber).IsUnique();

            // Ограничения для строк
            builder.Property(s => s.InventoryNumber)
                   .HasMaxLength(50)
                   .IsRequired();

            // Например, для Genus, Species, Cultivar и т.д. (каждый <= 50 символов)
            builder.Property(s => s.Genus)
                   .HasMaxLength(50);

            builder.Property(s => s.Species)
                   .HasMaxLength(50);

            builder.Property(s => s.Cultivar)
                   .HasMaxLength(50);

            builder.Property(s => s.Form)
                   .HasMaxLength(50);

            builder.Property(s => s.Synonyms)
                   .HasMaxLength(250);

            builder.Property(s => s.DeterminedBy)
                   .HasMaxLength(50);

            builder.Property(s => s.SampleOrigin)
                   .HasMaxLength(250);

            builder.Property(s => s.NaturalRange)
                   .HasMaxLength(250);

            builder.Property(s => s.EcologyAndBiology)
                   .HasMaxLength(250);

            builder.Property(s => s.EconomicUse)
                   .HasMaxLength(250);

            builder.Property(s => s.ConservationStatus)
                   .HasMaxLength(50);

            builder.Property(s => s.DuplicatesInfo)
                   .HasMaxLength(50);

            builder.Property(s => s.OriginalBreeder)
                   .HasMaxLength(250);

            builder.Property(s => s.Country)
                   .HasMaxLength(250);

            builder.Property(s => s.Illustration)
                   .HasMaxLength(250);

            builder.Property(s => s.Notes).
                HasMaxLength(5000);


            builder.Property(s => s.FilledBy)
                   .HasMaxLength(50);

            // Связь с Family (Many-to-One)
            builder.HasOne(s => s.Family)
                   .WithMany(f => f.Specimens)
                   .HasForeignKey(s => s.FamilyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Связь с Exposition (One-to-Many)
            builder.HasOne(s => s.Exposition)
                   .WithMany(e => e.Specimens)
                   .HasForeignKey(s => s.ExpositionId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Связь с User (Many-to-One)
            builder.HasOne(s => s.CreatedByUser)
                   .WithMany(u => u.ManagedSpecimens)
                   .HasForeignKey(s => s.CreatedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Настройки для полей даты создания и обновления
            builder.Property(s => s.CreatedAt)
                   .IsRequired();
                   
            builder.Property(s => s.LastUpdatedAt);

            // Связь с Phenology/ Biometry задается со стороны PhenologyConfiguration / BiometryConfiguration,
            // но можно и здесь, если хотим уточнить.

            // Настройка поля LocationType с перечислением
            builder.Property(s => s.LocationType)
                .HasConversion<int>()
                .HasDefaultValue(BGarden.Domain.Enums.LocationType.None)
                .IsRequired();
        }
    }
} 