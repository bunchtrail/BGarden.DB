using System.Collections.Generic;
using System.Threading.Tasks;
using BGarden.Application.DTO;
using Microsoft.AspNetCore.Http;

namespace BGarden.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с картами
    /// </summary>
    public interface IMapService
    {
        /// <summary>
        /// Получить все карты
        /// </summary>
        Task<IEnumerable<MapDto>> GetAllMapsAsync();
        
        /// <summary>
        /// Получить только активные карты
        /// </summary>
        Task<IEnumerable<MapDto>> GetActiveMapsAsync();
        
        /// <summary>
        /// Получить карту по идентификатору
        /// </summary>
        Task<MapDto> GetMapByIdAsync(int id);
        
        /// <summary>
        /// Получить карту вместе с растениями
        /// </summary>
        Task<MapDto> GetMapWithSpecimensAsync(int id);
        
        /// <summary>
        /// Создать новую карту
        /// </summary>
        Task<MapDto> CreateMapAsync(CreateMapDto mapDto);
        
        /// <summary>
        /// Обновить существующую карту
        /// </summary>
        Task<MapDto> UpdateMapAsync(int id, UpdateMapDto mapDto);
        
        /// <summary>
        /// Загрузить файл карты
        /// </summary>
        Task<MapDto> UploadMapFileAsync(int id, IFormFile file);
        
        /// <summary>
        /// Удалить карту
        /// </summary>
        Task DeleteMapAsync(int id);
    }
} 