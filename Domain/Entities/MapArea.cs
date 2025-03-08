using System;
using System.Collections.Generic;
using BGarden.DB.Domain.Enums;
using BGarden.Domain.Entities;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Область на карте ботанического сада (зона, сектор, экспозиция)
    /// </summary>
    public class MapArea
    {
        /// <summary>
        /// Уникальный идентификатор области
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
        /// Связанные координаты для построения полигона области
        /// </summary>
        public ICollection<MapAreaCoordinate> Coordinates { get; set; }

        /// <summary>
        /// Ссылка на экспозицию (если применимо)
        /// </summary>
        public int? ExpositionId { get; set; }

        /// <summary>
        /// Связанная экспозиция (если применимо)
        /// </summary>
        public Exposition Exposition { get; set; }

        /// <summary>
        /// Дата создания области
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления области
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 