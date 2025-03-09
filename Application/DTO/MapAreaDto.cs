using System;
using System.Collections.Generic;
using BGarden.DB.Domain.Enums;

namespace BGarden.DB.Application.DTO
{
    /// <summary>
    /// DTO для зоны карты
    /// </summary>
    public class MapAreaDto
    {
        /// <summary>
        /// Идентификатор зоны
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название зоны
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание зоны
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип зоны
        /// </summary>
        public AreaType Type { get; set; }

        /// <summary>
        /// GeoJSON полигона зоны
        /// </summary>
        public string GeoJson { get; set; }

        /// <summary>
        /// Цвет заливки (HEX формат)
        /// </summary>
        public string FillColor { get; set; }

        /// <summary>
        /// Цвет границы (HEX формат)
        /// </summary>
        public string StrokeColor { get; set; }

        /// <summary>
        /// Прозрачность заливки (0-1)
        /// </summary>
        public float FillOpacity { get; set; }

        /// <summary>
        /// Толщина границы
        /// </summary>
        public float StrokeWeight { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO для создания зоны карты
    /// </summary>
    public class CreateMapAreaDto
    {
        /// <summary>
        /// Название зоны
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание зоны
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип зоны
        /// </summary>
        public AreaType Type { get; set; }

        /// <summary>
        /// GeoJSON полигона зоны
        /// </summary>
        public string GeoJson { get; set; }

        /// <summary>
        /// Цвет заливки (HEX формат)
        /// </summary>
        public string FillColor { get; set; } = "#3388ff";

        /// <summary>
        /// Цвет границы (HEX формат)
        /// </summary>
        public string StrokeColor { get; set; } = "#3388ff";

        /// <summary>
        /// Прозрачность заливки (0-1)
        /// </summary>
        public float FillOpacity { get; set; } = 0.2f;

        /// <summary>
        /// Толщина границы
        /// </summary>
        public float StrokeWeight { get; set; } = 3.0f;
    }

    /// <summary>
    /// DTO для обновления зоны карты
    /// </summary>
    public class UpdateMapAreaDto
    {
        /// <summary>
        /// Идентификатор зоны
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название зоны
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание зоны
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип зоны
        /// </summary>
        public AreaType Type { get; set; }

        /// <summary>
        /// GeoJSON полигона зоны
        /// </summary>
        public string GeoJson { get; set; }

        /// <summary>
        /// Цвет заливки (HEX формат)
        /// </summary>
        public string FillColor { get; set; }

        /// <summary>
        /// Цвет границы (HEX формат)
        /// </summary>
        public string StrokeColor { get; set; }

        /// <summary>
        /// Прозрачность заливки (0-1)
        /// </summary>
        public float FillOpacity { get; set; }

        /// <summary>
        /// Толщина границы
        /// </summary>
        public float StrokeWeight { get; set; }
    }
} 