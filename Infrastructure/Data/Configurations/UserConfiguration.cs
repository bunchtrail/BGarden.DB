using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Первичный ключ
            builder.HasKey(u => u.Id);
            
            // Ограничения для строковых полей
            builder.Property(u => u.Username)
                   .HasMaxLength(50)
                   .IsRequired();
                   
            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(128)
                   .IsRequired();
                   
            builder.Property(u => u.PasswordSalt)
                   .HasMaxLength(128)
                   .IsRequired();
                   
            builder.Property(u => u.Email)
                   .HasMaxLength(100)
                   .IsRequired();
                   
            builder.Property(u => u.FullName)
                   .HasMaxLength(100)
                   .IsRequired();
                   
            builder.Property(u => u.Position)
                   .HasMaxLength(100);
                   
            // Уникальные индексы
            builder.HasIndex(u => u.Username)
                   .IsUnique();
                   
            builder.HasIndex(u => u.Email)
                   .IsUnique();
                   
            // Настройка для поля Role - хранить как int
            builder.Property(u => u.Role)
                   .HasConversion<int>()
                   .IsRequired();
                   
            // Настройка для CreatedAt (дата создания)
            builder.Property(u => u.CreatedAt)
                   .IsRequired();
                   
            // Связь с Specimen (One-to-Many)
            builder.HasMany(u => u.ManagedSpecimens)
                   .WithOne(s => s.CreatedByUser)
                   .HasForeignKey(s => s.CreatedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 