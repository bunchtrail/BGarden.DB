namespace BGarden.Domain.Enums
{
    /// <summary>
    /// Тип координат, используемых для определения местоположения образца
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// Местоположение не определено
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Географические координаты (широта/долгота)
        /// </summary>
        Geographic = 1,
        
        /// <summary>
        /// Координаты на схематической карте ботанического сада
        /// </summary>
        SchematicMap = 2
    }
} 