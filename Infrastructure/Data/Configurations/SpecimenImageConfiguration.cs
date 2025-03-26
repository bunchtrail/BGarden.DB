using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация для сущности SpecimenImage в Entity Framework Core
    /// </summary>
    public class SpecimenImageConfiguration : IEntityTypeConfiguration<SpecimenImage>
    {
        public void Configure(EntityTypeBuilder<SpecimenImage> builder)
        {
            builder.ToTable("SpecimenImages");
            
            builder.HasKey(si => si.Id);
            
            builder.Property(si => si.ImageData)
                .IsRequired()
                .HasColumnType("bytea");
                
            builder.Property(si => si.ContentType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(si => si.Description)
                .HasMaxLength(500);
                
            builder.Property(si => si.UploadedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            builder.HasOne(si => si.Specimen)
                .WithMany(s => s.SpecimenImages)
                .HasForeignKey(si => si.SpecimenId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Индекс для ускорения поиска основного изображения
            builder.HasIndex(si => new { si.SpecimenId, si.IsMain });
        }
    }
} 