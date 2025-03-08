using System;
using BGarden.DB.Domain.Enums;
using BGarden.Domain.Entities;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Маркер на карте ботанического сада
    /// </summary>
    public class MapMarker
    {
        /// <summary>
        /// Уникальный идентификатор маркера
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Широта (координата)
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота (координата)
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Заголовок маркера
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Тип маркера
        /// </summary>
        public MarkerType Type { get; set; }

        /// <summary>
        /// Описание маркера (опциональное)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// HTML-контент для всплывающего окна маркера (опциональный)
        /// </summary>
        public string PopupContent { get; set; }

        /// <summary>
        /// Идентификатор экземпляра растения (опциональный)
        /// </summary>
        public int? SpecimenId { get; set; }

        /// <summary>
        /// Связанный экземпляр растения
        /// </summary>
        public Specimen Specimen { get; set; }

        /// <summary>
        /// Дата создания маркера
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления маркера
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 