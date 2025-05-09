using Application.DTO;
using Domain.Entities;
using System;
using System.Text;

namespace Application.Mappers
{
    /// <summary>
    /// Маппер для конвертации между моделями и DTO изображений образцов
    /// </summary>
    public static class SpecimenImageMapper
    {
        /// <summary>
        /// Конвертирует модель SpecimenImage в DTO
        /// </summary>
        /// <param name="image">Модель изображения</param>
        /// <param name="baseApiUrl">Базовый URL для формирования ImageUrl</param>
        /// <returns>DTO изображения</returns>
        public static SpecimenImageDto ToDto(this SpecimenImage image, string baseApiUrl = "")
        {
            var dto = new SpecimenImageDto
            {
                Id = image.Id,
                SpecimenId = image.SpecimenId,
                RelativeFilePath = image.FilePath,
                OriginalFileName = image.OriginalFileName,
                FileSize = image.FileSize,
                ContentType = image.ContentType,
                Description = image.Description,
                IsMain = image.IsMain,
                UploadedAt = image.UploadedAt
            };
            
            return dto;
        }
        
        /// <summary>
        /// Конвертирует DTO создания изображения в модель
        /// </summary>
        /// <param name="dto">DTO создания изображения</param>
        /// <returns>Модель изображения</returns>
        public static SpecimenImage ToEntity(this CreateSpecimenImageDto dto)
        {
            return new SpecimenImage
            {
                SpecimenId = dto.SpecimenId,
                FilePath = dto.FilePath,
                OriginalFileName = dto.OriginalFileName,
                FileSize = dto.FileSize,
                ContentType = dto.ContentType,
                Description = dto.Description,
                IsMain = dto.IsMain,
                UploadedAt = DateTime.UtcNow
            };
        }
        
        /// <summary>
        /// Применяет обновление из DTO к существующей модели
        /// </summary>
        /// <param name="entity">Существующая модель изображения</param>
        /// <param name="dto">DTO обновления</param>
        /// <returns>Обновленная модель изображения</returns>
        public static SpecimenImage ApplyUpdate(this SpecimenImage entity, UpdateSpecimenImageDto dto)
        {
            if (dto.Description != null)
            {
                entity.Description = dto.Description;
            }
            if (dto.IsMain.HasValue)
            {
                entity.IsMain = dto.IsMain.Value;
            }

            return entity;
        }
    }
} 