using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.DTO
{
    /// <summary>
    /// DTO для пакетной загрузки изображений образцов
    /// </summary>
    public class BatchImageUploadDto
    {
        /// <summary>
        /// Идентификатор образца растения
        /// </summary>
        [Required]
        public int SpecimenId { get; set; }
        
        /// <summary>
        /// Флаг, указывающий, является ли изображение основным
        /// </summary>
        public bool IsMain { get; set; }
        
        /// <summary>
        /// Коллекция файлов изображений для загрузки
        /// </summary>
        [Required]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }

    /// <summary>
    /// Атрибут для валидации максимального количества файлов
    /// </summary>
    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxFiles;
        
        public MaxFileCountAttribute(int maxFiles)
        {
            _maxFiles = maxFiles;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IList<IFormFile> files && files.Count > _maxFiles)
            {
                return new ValidationResult($"Максимальное количество файлов: {_maxFiles}");
            }
            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Атрибут для валидации максимального размера файла
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxSize)
            {
                return new ValidationResult($"Максимальный размер файла: {_maxSize/1024/1024}MB");
            }
            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Атрибут для валидации расширений файлов
    /// </summary>
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !_extensions.Contains(extension))
                {
                    return new ValidationResult($"Допустимые расширения файлов: {string.Join(", ", _extensions)}");
                }
            }
            return ValidationResult.Success;
        }
    }
} 