using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Первичный ключ
            builder.HasKey(r => r.Id);
            
            // Ограничения для строковых полей
            builder.Property(r => r.Token)
                   .HasMaxLength(128)
                   .IsRequired();
                   
            builder.Property(r => r.CreatedByIp)
                   .HasMaxLength(50);
                   
            builder.Property(r => r.RevokedByIp)
                   .HasMaxLength(50);
                   
            builder.Property(r => r.ReplacedByToken)
                   .HasMaxLength(128);
            
            // Настройка для дат
            builder.Property(r => r.ExpiryDate)
                   .IsRequired();
                   
            builder.Property(r => r.CreatedAt)
                   .IsRequired();
            
            // Индексы для быстрого поиска
            builder.HasIndex(r => r.Token)
                   .IsUnique();
                   
            builder.HasIndex(r => new { r.UserId, r.IsRevoked });
            
            // Связь с User (Many-to-One)
            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 