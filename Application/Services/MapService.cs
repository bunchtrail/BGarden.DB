using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BGarden.Application.DTO;
using BGarden.Application.Interfaces;
using BGarden.Application.Mappers;
using BGarden.Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BGarden.Application.Services
{
    /// <summary>
    /// Сервис для работы с картами
    /// </summary>
    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MapService> _logger;
        private readonly string _mapsFolder;

        public MapService(
            IMapRepository mapRepository,
            IUnitOfWork unitOfWork,
            ILogger<MapService> logger,
            string mapsFolder = null)
        {
            _mapRepository = mapRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            
            // Определяем корневую папку приложения, исключая bin/Debug/net*
            string appRootPath;
            if (mapsFolder != null)
            {
                // Используем переданный путь, если он указан
                _mapsFolder = mapsFolder;
            }
            else
            {
                // Получаем базовый путь исполнения и корректируем его
                appRootPath = AppContext.BaseDirectory;
                
                // Если путь содержит bin/Debug или bin/Release, обрезаем его до корня приложения
                if (appRootPath.Contains(Path.Combine("bin", "Debug")) || appRootPath.Contains(Path.Combine("bin", "Release")))
                {
                    // Возвращаемся на два уровня вверх, чтобы получить корень приложения
                    var binIndex = appRootPath.IndexOf(Path.Combine("bin"));
                    if (binIndex > 0)
                    {
                        appRootPath = appRootPath.Substring(0, binIndex);
                    }
                }
                
                _mapsFolder = Path.Combine(appRootPath, "wwwroot", "maps");
                _logger.LogInformation("Maps folder path: {MapsFolder}", _mapsFolder);
            }
            
            // Создаем директорию для хранения карт, если её нет
            if (!Directory.Exists(_mapsFolder))
            {
                Directory.CreateDirectory(_mapsFolder);
                _logger.LogInformation("Created maps directory: {MapsFolder}", _mapsFolder);
            }
        }

        /// <summary>
        /// Получить все карты
        /// </summary>
        public async Task<IEnumerable<MapDto>> GetAllMapsAsync()
        {
            var maps = await _mapRepository.GetAllAsync();
            return maps.Select(m => m.ToDto());
        }

        /// <summary>
        /// Получить только активные карты
        /// </summary>
        public async Task<IEnumerable<MapDto>> GetActiveMapsAsync()
        {
            var maps = await _mapRepository.GetActiveMapsByAsync();
            return maps.Select(m => m.ToDto());
        }

        /// <summary>
        /// Получить карту по идентификатору
        /// </summary>
        public async Task<MapDto> GetMapByIdAsync(int id)
        {
            var map = await _mapRepository.GetByIdAsync(id);
            if (map == null)
            {
                _logger.LogWarning("Карта с ID {Id} не найдена", id);
                return null;
            }

            var specimensCount = await _mapRepository.GetSpecimensCountByMapIdAsync(id);
            return map.ToDto(specimensCount);
        }

        /// <summary>
        /// Получить карту вместе с растениями
        /// </summary>
        public async Task<MapDto> GetMapWithSpecimensAsync(int id)
        {
            var map = await _mapRepository.GetMapWithSpecimensAsync(id);
            if (map == null)
            {
                _logger.LogWarning("Карта с ID {Id} не найдена", id);
                return null;
            }

            return map.ToDto();
        }

        /// <summary>
        /// Создать новую карту
        /// </summary>
        public async Task<MapDto> CreateMapAsync(CreateMapDto mapDto)
        {
            if (mapDto == null)
                throw new ArgumentNullException(nameof(mapDto));

            var nameExists = await _mapRepository.ExistsByNameAsync(mapDto.Name);
            if (nameExists)
            {
                _logger.LogWarning("Карта с названием {Name} уже существует", mapDto.Name);
                throw new InvalidOperationException($"Карта с названием {mapDto.Name} уже существует");
            }

            var map = mapDto.ToEntity();
            
            // Устанавливаем временные значения для обязательных полей
            map.FilePath = "/maps/placeholder.png"; // Временный путь
            map.ContentType = "image/png";
            map.FileSize = 0;
            map.UploadDate = DateTime.UtcNow;
            map.LastUpdated = DateTime.UtcNow;
            map.IsActive = true;
            
            await _mapRepository.AddAsync(map);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Создана новая карта: {MapName} (ID: {MapId})", map.Name, map.Id);
            return map.ToDto();
        }

        /// <summary>
        /// Обновить существующую карту
        /// </summary>
        public async Task<MapDto> UpdateMapAsync(int id, UpdateMapDto mapDto)
        {
            if (mapDto == null)
                throw new ArgumentNullException(nameof(mapDto));

            var map = await _mapRepository.GetByIdAsync(id);
            if (map == null)
            {
                _logger.LogWarning("Карта с ID {Id} не найдена", id);
                return null;
            }

            if (mapDto.Name != null && mapDto.Name != map.Name)
            {
                var nameExists = await _mapRepository.ExistsByNameAsync(mapDto.Name);
                if (nameExists)
                {
                    _logger.LogWarning("Карта с названием {Name} уже существует", mapDto.Name);
                    throw new InvalidOperationException($"Карта с названием {mapDto.Name} уже существует");
                }
            }

            map.UpdateFromDto(mapDto);
            _mapRepository.Update(map);
            await _unitOfWork.SaveChangesAsync();

            var specimensCount = await _mapRepository.GetSpecimensCountByMapIdAsync(id);
            _logger.LogInformation("Обновлена карта: {MapName} (ID: {MapId})", map.Name, map.Id);
            return map.ToDto(specimensCount);
        }

        /// <summary>
        /// Загрузить файл карты
        /// </summary>
        public async Task<MapDto> UploadMapFileAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не выбран или пустой", nameof(file));

            var map = await _mapRepository.GetByIdAsync(id);
            if (map == null)
            {
                _logger.LogWarning("Карта с ID {Id} не найдена", id);
                return null;
            }

            // Проверяем тип файла (разрешаем только изображения)
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/svg+xml" };
            if (!allowedContentTypes.Contains(file.ContentType))
            {
                _logger.LogWarning("Недопустимый тип файла: {ContentType}", file.ContentType);
                throw new InvalidOperationException($"Недопустимый тип файла: {file.ContentType}. Разрешены только изображения.");
            }

            // Удаляем старый файл, если он существует
            if (!string.IsNullOrEmpty(map.FilePath))
            {
                // Получаем имя файла из относительного пути
                var oldFileName = Path.GetFileName(map.FilePath);
                var oldFilePath = Path.Combine(_mapsFolder, oldFileName);
                
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                    _logger.LogInformation("Удален старый файл карты: {OldFilePath}", oldFilePath);
                }
            }

            // Формируем имя файла на основе ID карты и расширения
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{map.Id:D5}_{DateTime.UtcNow:yyyyMMdd_HHmmss}{fileExtension}";
            var filePath = Path.Combine(_mapsFolder, fileName);

            _logger.LogInformation("Сохранение файла карты в: {FilePath}", filePath);

            try
            {
                // Сохраняем файл
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                _logger.LogInformation("Файл успешно сохранен: {FileName}, размер: {FileSize} байт", fileName, file.Length);

                // Обновляем информацию о карте
                map.FilePath = $"/maps/{fileName}"; // Относительный путь для веб-приложения
                map.ContentType = file.ContentType;
                map.FileSize = file.Length;
                map.LastUpdated = DateTime.UtcNow;

                _mapRepository.Update(map);
                await _unitOfWork.SaveChangesAsync();
                
                // Проверяем, существует ли файл после сохранения
                if (File.Exists(filePath))
                {
                    _logger.LogInformation("Проверка файла: файл успешно создан по пути {FilePath}", filePath);
                }
                else
                {
                    _logger.LogWarning("Проверка файла: файл не найден по пути {FilePath} после сохранения!", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении файла карты: {FilePath}", filePath);
                throw new InvalidOperationException($"Ошибка при сохранении файла: {ex.Message}", ex);
            }

            var specimensCount = await _mapRepository.GetSpecimensCountByMapIdAsync(id);
            _logger.LogInformation("Загружен файл карты для: {MapName} (ID: {MapId})", map.Name, map.Id);
            return map.ToDto(specimensCount);
        }

        /// <summary>
        /// Удалить карту
        /// </summary>
        public async Task DeleteMapAsync(int id)
        {
            var map = await _mapRepository.GetByIdAsync(id);
            if (map == null)
            {
                _logger.LogWarning("Карта с ID {Id} не найдена", id);
                return;
            }
            
            // Проверим, есть ли растения на карте
            var specimensCount = await _mapRepository.GetSpecimensCountByMapIdAsync(id);
            if (specimensCount > 0)
            {
                throw new InvalidOperationException($"Невозможно удалить карту, так как на ней размещено {specimensCount} растений");
            }
            
            // Удаляем файл карты, если он существует
            if (!string.IsNullOrEmpty(map.FilePath))
            {
                var fullPath = Path.Combine(AppContext.BaseDirectory, map.FilePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            
            _mapRepository.Remove(map);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 