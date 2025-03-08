using System;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Координата для области на карте
    /// </summary>
    public class MapAreaCoordinate
    {
        /// <summary>
        /// Уникальный идентификатор координаты
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор связанной области
        /// </summary>
        public int MapAreaId { get; set; }

        /// <summary>
        /// Связанная область
        /// </summary>
        public MapArea MapArea { get; set; }

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
} 