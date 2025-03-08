using System;

namespace BGarden.DB.Domain.Enums
{
    /// <summary>
    /// Типы маркеров на карте ботанического сада
    /// </summary>
    public enum MarkerType
    {
        /// <summary>
        /// Растение
        /// </summary>
        Plant,
        
        /// <summary>
        /// Экспозиция
        /// </summary>
        Exposition,
        
        /// <summary>
        /// Объект инфраструктуры
        /// </summary>
        Facility,
        
        /// <summary>
        /// Вход
        /// </summary>
        Entrance,
        
        /// <summary>
        /// Другое
        /// </summary>
        Other
    }
} 