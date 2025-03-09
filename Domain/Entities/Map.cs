using BGarden.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Сущность, представляющая карту объекта
    /// </summary>
    public class Map
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
        /// Тип файла карты (например, "image/jpeg", "image/png")
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
        /// Флаг активности (используется ли карта в настоящее время)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Связь с экземплярами растений, размещенными на этой карте
        /// </summary>
        public ICollection<Specimen> Specimens { get; set; }
    }
} 