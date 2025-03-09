using System;
using BGarden.Application.DTO;
using Domain.Entities;

namespace BGarden.Application.Mappers
{
    /// <summary>
    /// Маппер для преобразования между сущностью Map и DTO
    /// </summary>
    public static class MapMapper
    {
        /// <summary>
        /// Преобразовать сущность Map в DTO
        /// </summary>
        public static MapDto ToDto(this Map map, int? specimensCount = null)
        {
            if (map == null)
                return null;

            return new MapDto
            {
                Id = map.Id,
                Name = map.Name,
                Description = map.Description,
                FilePath = map.FilePath,
                ContentType = map.ContentType,
                FileSize = map.FileSize,
                UploadDate = map.UploadDate,
                LastUpdated = map.LastUpdated,
                IsActive = map.IsActive,
                SpecimensCount = specimensCount ?? map.Specimens?.Count ?? 0
            };
        }

        /// <summary>
        /// Создать новую сущность Map из DTO
        /// </summary>
        public static Map ToEntity(this CreateMapDto mapDto)
        {
            if (mapDto == null)
                return null;

            return new Map
            {
                Name = mapDto.Name,
                Description = mapDto.Description,
                IsActive = true,
                UploadDate = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Обновить существующую сущность Map из DTO
        /// </summary>
        public static void UpdateFromDto(this Map map, UpdateMapDto mapDto)
        {
            if (map == null || mapDto == null)
                return;

            if (mapDto.Name != null)
                map.Name = mapDto.Name;

            if (mapDto.Description != null)
                map.Description = mapDto.Description;

            if (mapDto.IsActive.HasValue)
                map.IsActive = mapDto.IsActive.Value;

            map.LastUpdated = DateTime.UtcNow;
        }
    }
} 