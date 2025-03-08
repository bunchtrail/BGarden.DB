using System;

namespace BGarden.DB.Application.DTO
{
    /// <summary>
    /// DTO для настроек карты
    /// </summary>
    public class MapOptionsDto
    {
        /// <summary>
        /// Идентификатор настроек
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название конфигурации карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Широта центра карты
        /// </summary>
        public double CenterLatitude { get; set; }

        /// <summary>
        /// Долгота центра карты
        /// </summary>
        public double CenterLongitude { get; set; }

        /// <summary>
        /// Уровень масштабирования
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        /// Минимальный уровень масштабирования
        /// </summary>
        public int? MinZoom { get; set; }

        /// <summary>
        /// Максимальный уровень масштабирования
        /// </summary>
        public int? MaxZoom { get; set; }

        /// <summary>
        /// Южная граница (опционально)
        /// </summary>
        public double? SouthBound { get; set; }

        /// <summary>
        /// Западная граница (опционально)
        /// </summary>
        public double? WestBound { get; set; }

        /// <summary>
        /// Северная граница (опционально)
        /// </summary>
        public double? NorthBound { get; set; }

        /// <summary>
        /// Восточная граница (опционально)
        /// </summary>
        public double? EastBound { get; set; }

        /// <summary>
        /// Указывает, является ли эта конфигурация активной по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// DTO для создания настроек карты
    /// </summary>
    public class CreateMapOptionsDto
    {
        /// <summary>
        /// Название конфигурации карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Широта центра карты
        /// </summary>
        public double CenterLatitude { get; set; }

        /// <summary>
        /// Долгота центра карты
        /// </summary>
        public double CenterLongitude { get; set; }

        /// <summary>
        /// Уровень масштабирования
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        /// Минимальный уровень масштабирования
        /// </summary>
        public int? MinZoom { get; set; }

        /// <summary>
        /// Максимальный уровень масштабирования
        /// </summary>
        public int? MaxZoom { get; set; }

        /// <summary>
        /// Южная граница (опционально)
        /// </summary>
        public double? SouthBound { get; set; }

        /// <summary>
        /// Западная граница (опционально)
        /// </summary>
        public double? WestBound { get; set; }

        /// <summary>
        /// Северная граница (опционально)
        /// </summary>
        public double? NorthBound { get; set; }

        /// <summary>
        /// Восточная граница (опционально)
        /// </summary>
        public double? EastBound { get; set; }

        /// <summary>
        /// Указывает, является ли эта конфигурация активной по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// DTO для обновления настроек карты
    /// </summary>
    public class UpdateMapOptionsDto : CreateMapOptionsDto
    {
        /// <summary>
        /// Идентификатор настроек для обновления
        /// </summary>
        public int Id { get; set; }
    }
} 