using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace BGarden.Domain.Entities
{
    /// <summary>
    /// Сущность, описывающая область (регион, зону, оранжерею) в ботаническом саду
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Уникальный идентификатор (первичный ключ)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название области (например, "Оранжерея №1", "Сад камней" и т.д.)
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание области
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Координаты центра области (широта)
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Координаты центра области (долгота)
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Геометрическая точка, представляющая центр области
        /// </summary>
        public Point? Location { get; set; }

        /// <summary>
        /// Радиус области в метрах (если область приблизительно круглая)
        /// </summary>
        public decimal? Radius { get; set; }

        /// <summary>
        /// Многоугольник, описывающий границы области (в формате Well-known text)
        /// Например: "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))"
        /// </summary>
        public string? BoundaryWkt { get; set; }

        /// <summary>
        /// Геометрический многоугольник, описывающий границы области
        /// </summary>
        public Polygon? Boundary { get; set; }

        /// <summary>
        /// Список координат для построения полигона в формате GeoJSON
        /// Пример: [[lon1,lat1],[lon2,lat2],[lon3,lat3],[lon1,lat1]]
        /// </summary>
        public string? PolygonCoordinates { get; set; }

        /// <summary>
        /// Цвет границы области в формате HEX или RGBA
        /// </summary>
        public string? StrokeColor { get; set; }

        /// <summary>
        /// Цвет заливки области в формате HEX или RGBA
        /// </summary>
        public string? FillColor { get; set; }

        /// <summary>
        /// Прозрачность заливки (от 0 до 1)
        /// </summary>
        public decimal? FillOpacity { get; set; }

        /// <summary>
        /// Тип сектора, к которому относится область
        /// </summary>
        public BGarden.Domain.Enums.SectorType SectorType { get; set; }

        /// <summary>
        /// Дата создания области
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления области
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Образцы растений в данной области
        /// </summary>
        public ICollection<Specimen> Specimens { get; set; } = new List<Specimen>();
    }
} 