using System;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Базовые настройки карты ботанического сада для Leaflet
    /// </summary>
    public class MapOptions
    {
        /// <summary>
        /// Уникальный идентификатор настроек
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название конфигурации карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Широта центра карты по умолчанию
        /// </summary>
        public double CenterLatitude { get; set; }

        /// <summary>
        /// Долгота центра карты по умолчанию
        /// </summary>
        public double CenterLongitude { get; set; }

        /// <summary>
        /// Уровень масштабирования по умолчанию
        /// </summary>
        public int DefaultZoom { get; set; }

        /// <summary>
        /// Минимальный уровень масштабирования
        /// </summary>
        public int MinZoom { get; set; } = 1;

        /// <summary>
        /// Максимальный уровень масштабирования
        /// </summary>
        public int MaxZoom { get; set; } = 18;

        /// <summary>
        /// Путь к файлу схемы карты
        /// </summary>
        public string MapSchemaUrl { get; set; }

        /// <summary>
        /// Указывает, является ли эта конфигурация активной по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Дата создания настроек
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления настроек
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 