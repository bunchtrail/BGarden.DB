using System;

namespace BGarden.DB.Domain.Enums
{
    /// <summary>
    /// Типы зон на карте
    /// </summary>
    [Flags]
    public enum AreaType
    {
        /// <summary>
        /// Неопределенный тип
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Экспозиция
        /// </summary>
        Exposition = 1,

        /// <summary>
        /// Сектор
        /// </summary>
        Sector = 2,

        /// <summary>
        /// Зона обслуживания
        /// </summary>
        Service = 4,

        /// <summary>
        /// Территория
        /// </summary>
        Territory = 8,

        /// <summary>
        /// Тропа
        /// </summary>
        Path = 16,

        /// <summary>
        /// Водоем
        /// </summary>
        Water = 32
    }
} 