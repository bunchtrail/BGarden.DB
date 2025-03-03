namespace BGarden.Domain.Enums
{
    /// <summary>
    /// Типы событий аутентификации и авторизации
    /// </summary>
    public enum AuthEventType
    {
        /// <summary>
        /// Успешный вход в систему
        /// </summary>
        Login = 1,
        
        /// <summary>
        /// Выход из системы
        /// </summary>
        Logout = 2,
        
        /// <summary>
        /// Смена пароля
        /// </summary>
        PasswordChange = 3,
        
        /// <summary>
        /// Неудачная попытка входа
        /// </summary>
        FailedLogin = 4,
        
        /// <summary>
        /// Блокировка аккаунта
        /// </summary>
        AccountLocked = 5,
        
        /// <summary>
        /// Разблокировка аккаунта
        /// </summary>
        AccountUnlocked = 6,
        
        /// <summary>
        /// Создание пользователя
        /// </summary>
        UserCreated = 7,
        
        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        UserUpdated = 8,
        
        /// <summary>
        /// Двухфакторная аутентификация
        /// </summary>
        TwoFactorAuthentication = 9
    }
} 