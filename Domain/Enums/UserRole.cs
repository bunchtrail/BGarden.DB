namespace BGarden.Domain.Enums
{
    /// <summary>
    /// Перечисление ролей пользователей системы
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Администратор системы (полный доступ)
        /// </summary>
        Administrator = 1,
        
        /// <summary>
        /// Работник ботанического сада (расширенный доступ)
        /// </summary>
        Employee = 2,
        
        /// <summary>
        /// Клиент (ограниченный доступ, только для просмотра)
        /// </summary>
        Client = 3
    }
} 