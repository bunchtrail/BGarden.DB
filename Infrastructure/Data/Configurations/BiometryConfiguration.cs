using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class BiometryConfiguration : IEntityTypeConfiguration<Biometry>
    {
        public void Configure(EntityTypeBuilder<Biometry> builder)
        {
            builder.HasKey(b => b.Id);

            // Связь с Specimen
            builder.HasOne(b => b.Specimen)
                   .WithMany(s => s.Biometries)
                   .HasForeignKey(b => b.SpecimenId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.Notes)
                   .HasMaxLength(500);
        }
    }
} 