using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

namespace BGarden.Infrastructure.Data.Configurations
{
    public class AuthLogConfiguration : IEntityTypeConfiguration<AuthLog>
    {
        public void Configure(EntityTypeBuilder<AuthLog> builder)
        {
            // Первичный ключ
            builder.HasKey(a => a.Id);
            
            // Ограничения для строковых полей
            builder.Property(a => a.Username)
                   .HasMaxLength(50)
                   .IsRequired();
                   
            builder.Property(a => a.IpAddress)
                   .HasMaxLength(50);
                   
            builder.Property(a => a.UserAgent)
                   .HasMaxLength(512);
                   
            builder.Property(a => a.Details)
                   .HasMaxLength(1024);
            
            // Настройка для поля EventType - хранить как int
            builder.Property(a => a.EventType)
                   .HasConversion<int>()
                   .IsRequired();
                   
            // Настройка для Timestamp
            builder.Property(a => a.Timestamp)
                   .IsRequired();
                   
            // Индекс для быстрого поиска по имени пользователя
            builder.HasIndex(a => a.Username);
            
            // Индекс для быстрого поиска по типу события
            builder.HasIndex(a => a.EventType);
            
            // Индекс для быстрого поиска по дате
            builder.HasIndex(a => a.Timestamp);
            
            // Связь с User (Many-to-One)
            builder.HasOne(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 