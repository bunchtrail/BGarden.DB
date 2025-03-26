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
        /// <param name="includeImageData">Флаг, указывающий, нужно ли включать данные изображения</param>
        /// <returns>DTO изображения</returns>
        public static SpecimenImageDto ToDto(this SpecimenImage image, bool includeImageData = true)
        {
            var dto = new SpecimenImageDto
            {
                Id = image.Id,
                SpecimenId = image.SpecimenId,
                ContentType = image.ContentType,
                Description = image.Description,
                IsMain = image.IsMain,
                UploadedAt = image.UploadedAt
            };
            
            if (includeImageData && image.ImageData != null)
            {
                dto.ImageDataBase64 = Convert.ToBase64String(image.ImageData);
            }
            
            return dto;
        }
        
        /// <summary>
        /// Конвертирует DTO создания изображения в модель
        /// </summary>
        /// <param name="dto">DTO создания изображения</param>
        /// <returns>Модель изображения</returns>
        public static SpecimenImage ToEntity(this CreateSpecimenImageDto dto)
        {
            byte[] imageData = Convert.FromBase64String(dto.ImageDataBase64);
            
            return new SpecimenImage
            {
                SpecimenId = dto.SpecimenId,
                ImageData = imageData,
                ContentType = dto.ContentType,
                Description = dto.Description,
                IsMain = dto.IsMain,
                UploadedAt = DateTime.UtcNow
            };
        }
        
        /// <summary>
        /// Конвертирует DTO создания изображения с бинарными данными в модель
        /// </summary>
        /// <param name="dto">DTO создания изображения с бинарными данными</param>
        /// <returns>Модель изображения</returns>
        public static SpecimenImage ToEntity(this CreateSpecimenImageBinaryDto dto)
        {
            return new SpecimenImage
            {
                SpecimenId = dto.SpecimenId,
                ImageData = dto.ImageData,
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
            entity.Description = dto.Description;
            entity.IsMain = dto.IsMain;
            
            return entity;
        }
    }
} 