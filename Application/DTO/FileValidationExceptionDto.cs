using System;

namespace Application.DTO.Exceptions
{
    /// <summary>
    /// Исключение при валидации файлов
    /// </summary>
    public class FileValidationException : Exception
    {
        /// <summary>
        /// Имя файла, вызвавшего ошибку
        /// </summary>
        public string? FileName { get; }
        
        /// <summary>
        /// Причина ошибки
        /// </summary>
        public string Reason { get; }
        
        /// <summary>
        /// Конструктор с сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public FileValidationException(string message) : base(message)
        {
            Reason = message;
        }
        
        /// <summary>
        /// Конструктор с именем файла и сообщением об ошибке
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="message">Сообщение об ошибке</param>
        public FileValidationException(string fileName, string message) 
            : base($"Ошибка валидации файла '{fileName}': {message}")
        {
            FileName = fileName;
            Reason = message;
        }
        
        /// <summary>
        /// Конструктор с именем файла, сообщением об ошибке и внутренним исключением
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="inner">Внутреннее исключение</param>
        public FileValidationException(string fileName, string message, Exception inner) 
            : base($"Ошибка валидации файла '{fileName}': {message}", inner)
        {
            FileName = fileName;
            Reason = message;
        }
    }
    
    /// <summary>
    /// Исключение при отсутствии ресурса
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Тип ресурса
        /// </summary>
        public string ResourceType { get; }
        
        /// <summary>
        /// Идентификатор ресурса
        /// </summary>
        public string ResourceId { get; }
        
        /// <summary>
        /// Конструктор с типом и идентификатором ресурса
        /// </summary>
        /// <param name="resourceType">Тип ресурса</param>
        /// <param name="resourceId">Идентификатор ресурса</param>
        public ResourceNotFoundException(string resourceType, string resourceId) 
            : base($"Ресурс типа '{resourceType}' с идентификатором '{resourceId}' не найден")
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }
    }
} 