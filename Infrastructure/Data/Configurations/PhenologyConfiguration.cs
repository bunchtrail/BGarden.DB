using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class PhenologyConfiguration : IEntityTypeConfiguration<Phenology>
    {
        public void Configure(EntityTypeBuilder<Phenology> builder)
        {
            builder.HasKey(p => p.Id);

            // Связь с Specimen
            builder.HasOne(p => p.Specimen)
                   .WithMany(s => s.Phenologies)
                   .HasForeignKey(p => p.SpecimenId)
                   .OnDelete(DeleteBehavior.Cascade);

            // при необходимости можно добавить индексы
            // builder.HasIndex(p => new { p.SpecimenId, p.Year }).IsUnique();

            builder.Property(p => p.Notes)
                   .HasMaxLength(500);
        }
    }
} 