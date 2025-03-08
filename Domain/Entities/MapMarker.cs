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
        public string? Description { get; set; }

        /// <summary>
        /// Идентификатор экземпляра растения
        /// Это обязательное поле, так как маркер всегда привязан к растению
        /// </summary>
        public int SpecimenId { get; set; }

        /// <summary>
        /// Связанный экземпляр растения
        /// </summary>
        public Specimen Specimen { get; set; }

        /// <summary>
        /// Идентификатор региона, в котором находится маркер
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Связанный регион
        /// </summary>
        public Region? Region { get; set; }

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