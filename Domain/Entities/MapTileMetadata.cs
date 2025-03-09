using System;

namespace BGarden.DB.Domain.Entities
{
    /// <summary>
    /// Метаданные о тайле карты
    /// </summary>
    public class MapTileMetadata
    {
        /// <summary>
        /// Уникальный идентификатор метаданных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор слоя карты
        /// </summary>
        public int MapLayerId { get; set; }

        /// <summary>
        /// Уровень масштабирования (zoom)
        /// </summary>
        public int ZoomLevel { get; set; }

        /// <summary>
        /// Колонка тайла (X координата)
        /// </summary>
        public int TileColumn { get; set; }

        /// <summary>
        /// Строка тайла (Y координата)
        /// </summary>
        public int TileRow { get; set; }

        /// <summary>
        /// Размер файла тайла в байтах
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Контрольная сумма файла для проверки целостности
        /// </summary>
        public string? Checksum { get; set; }

        /// <summary>
        /// Относительный путь к файлу тайла от базовой директории слоя
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Дата создания метаданных
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления метаданных
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Связанный слой карты
        /// </summary>
        public MapLayer MapLayer { get; set; }
    }
} 