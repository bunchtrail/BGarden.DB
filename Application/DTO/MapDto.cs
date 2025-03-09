using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BGarden.Application.DTO
{
    /// <summary>
    /// DTO для создания новой карты
    /// </summary>
    public class CreateMapDto
    {
        /// <summary>
        /// Название карты
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Описание карты
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующей карты
    /// </summary>
    public class UpdateMapDto
    {
        /// <summary>
        /// Название карты
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Описание карты
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Является ли карта активной
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO для возвращения информации о карте
    /// </summary>
    public class MapDto
    {
        /// <summary>
        /// Уникальный идентификатор карты
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание карты
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Путь к файлу карты
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Тип файла карты
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Дата загрузки карты
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Флаг активности
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Количество растений на карте
        /// </summary>
        public int SpecimensCount { get; set; }
    }
} 