using System;
using System.Collections.Generic;
using BGarden.DB.Domain.Enums;

namespace BGarden.DB.Application.DTO
{
    /// <summary>
    /// DTO для координаты области на карте
    /// </summary>
    public class MapAreaCoordinateDto
    {
        /// <summary>
        /// Широта (координата)
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота (координата)
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Порядок координаты в полигоне
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    /// DTO для области на карте
    /// </summary>
    public class MapAreaDto
    {
        /// <summary>
        /// Идентификатор области
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название области
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание области (опциональное)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип области
        /// </summary>
        public AreaType Type { get; set; }

        /// <summary>
        /// Цвет границы области в формате HEX или RGBA (опциональный)
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Цвет заливки области в формате HEX или RGBA (опциональный)
        /// </summary>
        public string FillColor { get; set; }

        /// <summary>
        /// Координаты для построения полигона области
        /// </summary>
        public IEnumerable<MapAreaCoordinateDto> Coordinates { get; set; }

        /// <summary>
        /// Ссылка на экспозицию (если применимо)
        /// </summary>
        public int? ExpositionId { get; set; }
    }

    /// <summary>
    /// DTO для создания области на карте
    /// </summary>
    public class CreateMapAreaDto
    {
        /// <summary>
        /// Название области
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание области (опциональное)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип области
        /// </summary>
        public AreaType Type { get; set; }

        /// <summary>
        /// Цвет границы области в формате HEX или RGBA (опциональный)
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Цвет заливки области в формате HEX или RGBA (опциональный)
        /// </summary>
        public string FillColor { get; set; }

        /// <summary>
        /// Координаты для построения полигона области
        /// </summary>
        public IEnumerable<MapAreaCoordinateDto> Coordinates { get; set; }

        /// <summary>
        /// Ссылка на экспозицию (если применимо)
        /// </summary>
        public int? ExpositionId { get; set; }
    }

    /// <summary>
    /// DTO для обновления области на карте
    /// </summary>
    public class UpdateMapAreaDto : CreateMapAreaDto
    {
        /// <summary>
        /// Идентификатор области для обновления
        /// </summary>
        public int Id { get; set; }
    }
} 