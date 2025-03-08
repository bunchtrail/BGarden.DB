using System;

namespace BGarden.DB.Application.DTO
{
    /// <summary>
    /// DTO для слоя карты
    /// </summary>
    public class MapLayerDto
    {
        /// <summary>
        /// Идентификатор слоя
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
    }

    /// <summary>
    /// DTO для создания слоя карты
    /// </summary>
    public class CreateMapLayerDto
    {
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
    }

    /// <summary>
    /// DTO для обновления слоя карты
    /// </summary>
    public class UpdateMapLayerDto : CreateMapLayerDto
    {
        /// <summary>
        /// Идентификатор слоя для обновления
        /// </summary>
        public int Id { get; set; }
    }
} 