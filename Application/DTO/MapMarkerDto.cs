using System;
using BGarden.DB.Domain.Enums;

namespace BGarden.DB.Application.DTO
{
    /// <summary>
    /// DTO для маркера на карте
    /// </summary>
    public class MapMarkerDto
    {
        /// <summary>
        /// Идентификатор маркера
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
        /// Идентификатор региона, в котором находится маркер (опциональный)
        /// </summary>
        public int? RegionId { get; set; }
        
        /// <summary>
        /// Дата создания маркера
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата последнего обновления маркера
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO для создания маркера на карте
    /// </summary>
    public class CreateMapMarkerDto
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
    }

    /// <summary>
    /// DTO для обновления маркера на карте
    /// </summary>
    public class UpdateMapMarkerDto : CreateMapMarkerDto
    {
        /// <summary>
        /// Идентификатор маркера для обновления
        /// </summary>
        public int Id { get; set; }
    }
} 