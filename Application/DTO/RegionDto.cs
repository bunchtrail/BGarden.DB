using System;
using System.Collections.Generic;
using BGarden.Domain.Enums;

namespace Application.DTO
{
    /// <summary>
    /// DTO для передачи данных о регионах (областях) ботанического сада
    /// </summary>
    public class RegionDto
    {
        /// <summary>
        /// Уникальный идентификатор
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
        /// Радиус области в метрах (если область приблизительно круглая)
        /// </summary>
        public decimal? Radius { get; set; }

        /// <summary>
        /// Многоугольник, описывающий границы области (в формате Well-known text)
        /// Например: "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))"
        /// </summary>
        public string? BoundaryWkt { get; set; }

        /// <summary>
        /// Тип сектора, к которому относится область
        /// </summary>
        public SectorType SectorType { get; set; }

        /// <summary>
        /// Количество образцов растений в данной области
        /// </summary>
        public int SpecimensCount { get; set; }
    }
} 