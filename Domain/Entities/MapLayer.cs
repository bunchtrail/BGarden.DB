using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Слой карты ботанического сада
    /// </summary>
    public class MapLayer
    {
        /// <summary>
        /// Уникальный идентификатор слоя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название слоя карты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание слоя карты
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Относительный путь к базовой директории с тайлами
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// Минимальный уровень масштабирования
        /// </summary>
        public int MinZoom { get; set; }

        /// <summary>
        /// Максимальный уровень масштабирования
        /// </summary>
        public int MaxZoom { get; set; }

        /// <summary>
        /// Формат тайлов (png, jpg и т.д.)
        /// </summary>
        public string TileFormat { get; set; } = "png";

        /// <summary>
        /// Указывает, активен ли слой
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Минимальная координата X границ карты
        /// </summary>
        public double? MinX { get; set; }
        
        /// <summary>
        /// Минимальная координата Y границ карты
        /// </summary>
        public double? MinY { get; set; }
        
        /// <summary>
        /// Максимальная координата X границ карты
        /// </summary>
        public double? MaxX { get; set; }
        
        /// <summary>
        /// Максимальная координата Y границ карты
        /// </summary>
        public double? MaxY { get; set; }

        /// <summary>
        /// Дата создания слоя
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления слоя
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Метаданные тайлов, относящиеся к этому слою
        /// </summary>
        public ICollection<MapTileMetadata> TileMetadata { get; set; } = new List<MapTileMetadata>();
        
        /// <summary>
        /// Получает границы карты как объект Envelope на основе координатных полей
        /// </summary>
        /// <returns>Envelope с границами карты или null, если границы не заданы</returns>
        public Envelope GetBounds()
        {
            if (MinX.HasValue && MinY.HasValue && MaxX.HasValue && MaxY.HasValue)
            {
                return new Envelope(MinX.Value, MaxX.Value, MinY.Value, MaxY.Value);
            }
            return null;
        }
        
        /// <summary>
        /// Устанавливает границы карты из объекта Envelope
        /// </summary>
        /// <param name="envelope">Объект Envelope с границами карты</param>
        public void SetBounds(Envelope envelope)
        {
            if (envelope != null)
            {
                MinX = envelope.MinX;
                MinY = envelope.MinY;
                MaxX = envelope.MaxX;
                MaxY = envelope.MaxY;
            }
            else
            {
                MinX = null;
                MinY = null;
                MaxX = null;
                MaxY = null;
            }
        }
    }
} 