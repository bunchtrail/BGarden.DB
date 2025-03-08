using System;

namespace BGarden.DB.Domain.Enums
{
    /// <summary>
    /// Типы областей на карте ботанического сада
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// Сектор
        /// </summary>
        Sector,
        
        /// <summary>
        /// Экспозиция
        /// </summary>
        Exposition,
        
        /// <summary>
        /// Оранжерея
        /// </summary>
        Greenhouse,
        
        /// <summary>
        /// Зона ограниченного доступа
        /// </summary>
        Restricted
    }
} 