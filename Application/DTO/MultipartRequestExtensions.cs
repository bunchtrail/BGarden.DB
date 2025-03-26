using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Application.DTO.Extensions
{
    /// <summary>
    /// Расширения для работы с multipart/form-data запросами
    /// </summary>
    public static class MultipartRequestExtensions
    {
        /// <summary>
        /// Получает разделитель (boundary) из запроса multipart/form-data
        /// </summary>
        /// <param name="request">HTTP-запрос</param>
        /// <returns>Строка-разделитель</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если разделитель не найден</exception>
        public static string GetMultipartBoundary(this HttpRequest request)
        {
            if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader))
            {
                throw new InvalidOperationException("Неверный ContentType заголовок");
            }

            var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary).Value;
            
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidOperationException("Не указан разделитель (boundary) в запросе multipart/form-data");
            }

            return boundary;
        }
        
        /// <summary>
        /// Проверяет, является ли запрос запросом multipart/form-data
        /// </summary>
        /// <param name="request">HTTP-запрос</param>
        /// <returns>True, если запрос multipart/form-data</returns>
        public static bool IsMultipartContentType(this HttpRequest request)
        {
            return !string.IsNullOrEmpty(request.ContentType) && 
                   request.ContentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Проверяет, не превышает ли размер файла указанный лимит
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="maxSize">Максимальный размер в байтах</param>
        /// <returns>True, если размер файла не превышает лимит</returns>
        public static bool IsValidFileSize(this IFormFile file, long maxSize)
        {
            return file.Length <= maxSize;
        }
        
        /// <summary>
        /// Проверяет, имеет ли файл разрешенное расширение
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="allowedExtensions">Массив разрешенных расширений (с точкой)</param>
        /// <returns>True, если расширение файла разрешено</returns>
        public static bool HasValidExtension(this IFormFile file, string[] allowedExtensions)
        {
            var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && allowedExtensions.Contains(extension);
        }
    }
} 