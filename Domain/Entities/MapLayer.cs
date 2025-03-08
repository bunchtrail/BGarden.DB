using System;

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
        /// Уникальный строковый идентификатор слоя для использования на клиенте
        /// </summary>
        public string LayerId { get; set; }

        /// <summary>
        /// Название слоя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание слоя (опциональное)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL тайлового слоя
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Атрибуция для слоя (авторство, правовая информация)
        /// </summary>
        public string Attribution { get; set; }

        /// <summary>
        /// Указывает, является ли этот слой используемым по умолчанию
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Порядок отображения слоя
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Дата создания слоя
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления слоя
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 