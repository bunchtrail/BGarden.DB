using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class ExpositionConfiguration : IEntityTypeConfiguration<Exposition>
    {
        public void Configure(EntityTypeBuilder<Exposition> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            // One-to-Many Specimens:
            builder.HasMany(e => e.Specimens)
                   .WithOne(s => s.Exposition)
                   .HasForeignKey(s => s.ExpositionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 