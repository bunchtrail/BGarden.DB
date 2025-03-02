using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

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

            // Связь с Phenology/ Biometry задается со стороны PhenologyConfiguration / BiometryConfiguration,
            // но можно и здесь, если хотим уточнить.
        }
    }
} 