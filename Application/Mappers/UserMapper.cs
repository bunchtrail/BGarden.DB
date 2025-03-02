using System;
using BGarden.Application.DTO;
using BGarden.Domain.Entities;

namespace BGarden.Application.Mappers
{
    /// <summary>
    /// Маппер для преобразования между сущностью User и соответствующими DTO
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Преобразование сущности User в UserDto
        /// </summary>
        public static UserDto ToDto(this User user)
        {
            if (user == null)
                return null!;
                
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                Position = user.Position,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                IsActive = user.IsActive
            };
        }
        
        /// <summary>
        /// Преобразование CreateUserDto в сущность User
        /// </summary>
        public static User ToEntity(this CreateUserDto createUserDto)
        {
            if (createUserDto == null)
                return null!;
                
            return new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                FullName = createUserDto.FullName,
                Role = createUserDto.Role,
                Position = createUserDto.Position,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
        
        /// <summary>
        /// Обновление сущности User из UpdateUserDto
        /// </summary>
        public static void UpdateFromDto(this User user, UpdateUserDto updateUserDto)
        {
            if (user == null || updateUserDto == null)
                return;
                
            user.Email = updateUserDto.Email;
            user.FullName = updateUserDto.FullName;
            user.Role = updateUserDto.Role;
            user.Position = updateUserDto.Position;
            user.IsActive = updateUserDto.IsActive;
        }
    }
} 