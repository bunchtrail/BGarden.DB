using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class FamilyConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(f => f.Description)
                   .HasMaxLength(500);

            // Связь с Specimen (One-to-Many):
            builder.HasMany(f => f.Specimens)
                   .WithOne(s => s.Family)
                   .HasForeignKey(s => s.FamilyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 