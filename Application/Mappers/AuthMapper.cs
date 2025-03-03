using System;
using System.Collections.Generic;
using System.Linq;
using BGarden.Application.DTO;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;

namespace BGarden.Application.Mappers
{
    /// <summary>
    /// Маппер для преобразования между сущностями авторизации и DTO
    /// </summary>
    public static class AuthMapper
    {
        /// <summary>
        /// Преобразует RegisterDto в сущность User
        /// </summary>
        public static User ToEntity(this RegisterDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        /// <summary>
        /// Преобразует сущность AuthLog в AuthLogDto
        /// </summary>
        public static AuthLogDto ToDto(this AuthLog entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new AuthLogDto
            {
                Id = entity.Id,
                Username = entity.Username,
                UserId = entity.UserId,
                EventTypeId = (int)entity.EventType,
                EventTypeName = entity.EventType.ToString(),
                IpAddress = entity.IpAddress,
                UserAgent = entity.UserAgent,
                Timestamp = entity.Timestamp,
                Success = entity.Success,
                Details = entity.Details
            };
        }

        /// <summary>
        /// Преобразует коллекцию сущностей AuthLog в коллекцию AuthLogDto
        /// </summary>
        public static IEnumerable<AuthLogDto> ToDtos(this IEnumerable<AuthLog> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            return entities.Select(e => e.ToDto());
        }

        /// <summary>
        /// Создает DTO настройки двухфакторной аутентификации
        /// </summary>
        public static TwoFactorSetupDto CreateTwoFactorSetupDto(string secretKey, string qrCodeUrl, string username)
        {
            return new TwoFactorSetupDto
            {
                SecretKey = secretKey,
                QrCodeUrl = qrCodeUrl,
                Username = username
            };
        }

        /// <summary>
        /// Создает DTO токена аутентификации
        /// </summary>
        public static TokenDto CreateTokenDto(string accessToken, RefreshToken refreshToken, DateTime expiration, string username, bool requiresTwoFactor = false)
        {
            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Expiration = expiration,
                TokenType = "Bearer",
                Username = username,
                RequiresTwoFactor = requiresTwoFactor
            };
        }
    }
} 