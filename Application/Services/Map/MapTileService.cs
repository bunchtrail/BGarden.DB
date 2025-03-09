using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BGarden.DB.Domain.Entities;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

namespace BGarden.Application.Services.Map
{
    /// <summary>
    /// Сервис для работы с тайлами карты
    /// </summary>
    public class MapTileService : IMapTileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MapTileService> _logger;
        private readonly string _baseTilesDirectory;

        public MapTileService(IUnitOfWork unitOfWork, ILogger<MapTileService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            
            // Базовая директория для тайлов - корневая директория приложения/tiles
            _baseTilesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tiles");
            
            // Создаем директорию, если она не существует
            if (!Directory.Exists(_baseTilesDirectory))
            {
                Directory.CreateDirectory(_baseTilesDirectory);
            }
        }

        /// <inheritdoc/>
        public async Task<Stream> GetTileAsync(int layerId, int zoom, int x, int y)
        {
            try
            {
                // Получаем метаданные тайла
                var tileMetadata = await _unitOfWork.MapTileMetadata.GetTileMetadataAsync(layerId, zoom, x, y);
                if (tileMetadata == null)
                {
                    _logger.LogWarning($"Тайл не найден: layerId={layerId}, zoom={zoom}, x={x}, y={y}");
                    return null;
                }

                // Получаем слой
                var layer = await _unitOfWork.MapLayers.GetByIdAsync(layerId);
                if (layer == null)
                {
                    _logger.LogWarning($"Слой не найден: layerId={layerId}");
                    return null;
                }

                // Формируем полный путь к файлу тайла
                var tilePath = Path.Combine(_baseTilesDirectory, layer.BaseDirectory, tileMetadata.RelativePath);
                
                // Проверяем существование файла
                if (!File.Exists(tilePath))
                {
                    _logger.LogWarning($"Файл тайла не найден: {tilePath}");
                    return null;
                }

                // Открываем файл как поток для чтения
                return new FileStream(tilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении тайла: layerId={layerId}, zoom={zoom}, x={x}, y={y}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<MapLayer> CreateMapLayerFromTilesAsync(string name, string description, string baseDirectory, bool isActive = true)
        {
            try
            {
                // Проверяем, существует ли слой с таким именем
                var existingLayer = await _unitOfWork.MapLayers.FindByNameAsync(name);
                if (existingLayer != null)
                {
                    _logger.LogWarning($"Слой с именем '{name}' уже существует");
                    return existingLayer;
                }

                // Проверяем, существует ли директория с тайлами
                var fullPath = Path.Combine(_baseTilesDirectory, baseDirectory);
                if (!Directory.Exists(fullPath))
                {
                    _logger.LogError($"Директория с тайлами не найдена: {fullPath}");
                    return null;
                }

                // Определяем минимальный и максимальный уровень масштабирования
                var zoomLevels = Directory.GetDirectories(fullPath)
                    .Select(dir => Path.GetFileName(dir))
                    .Where(dir => int.TryParse(dir, out _))
                    .Select(dir => int.Parse(dir))
                    .ToList();

                if (zoomLevels.Count == 0)
                {
                    _logger.LogError($"В директории не найдены папки с уровнями масштабирования: {fullPath}");
                    return null;
                }

                int minZoom = zoomLevels.Min();
                int maxZoom = zoomLevels.Max();

                // Создаем новый слой
                var layer = new MapLayer
                {
                    Name = name,
                    Description = description,
                    BaseDirectory = baseDirectory,
                    MinZoom = minZoom,
                    MaxZoom = maxZoom,
                    TileFormat = "png", // По умолчанию используем PNG
                    IsActive = isActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Сохраняем слой в базу данных
                await _unitOfWork.MapLayers.AddAsync(layer);
                await _unitOfWork.SaveChangesAsync();

                // Индексируем тайлы
                await RefreshTileMetadataAsync(layer.Id);

                return layer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при создании слоя карты из тайлов: {name}, {baseDirectory}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<MapLayer> ProcessMapImageAsync(string name, string description, string inputImagePath, 
            string outputDirectory, int minZoom = 10, int maxZoom = 18, bool isActive = true)
        {
            try
            {
                // Проверяем, существует ли исходное изображение
                if (!File.Exists(inputImagePath))
                {
                    _logger.LogError($"Исходное изображение не найдено: {inputImagePath}");
                    return null;
                }

                // Создаем директорию для тайлов, если она не существует
                var fullOutputPath = Path.Combine(_baseTilesDirectory, outputDirectory);
                if (!Directory.Exists(fullOutputPath))
                {
                    Directory.CreateDirectory(fullOutputPath);
                }

                // Формируем команду для gdal2tiles
                var command = $"gdal2tiles.py -z {minZoom}-{maxZoom} -w leaflet -p mercator \"{inputImagePath}\" \"{fullOutputPath}\"";
                
                // Создаем процесс для запуска gdal2tiles
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "python", // Или "python3" в зависимости от системы
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                _logger.LogInformation($"Запуск процесса обработки изображения: {command}");

                // Запускаем процесс
                using (var process = Process.Start(processStartInfo))
                {
                    // Ожидаем завершения процесса
                    await process.WaitForExitAsync();

                    // Проверяем код завершения
                    if (process.ExitCode != 0)
                    {
                        var error = await process.StandardError.ReadToEndAsync();
                        _logger.LogError($"Ошибка при обработке изображения: {error}");
                        return null;
                    }

                    // Читаем вывод процесса
                    var output = await process.StandardOutput.ReadToEndAsync();
                    _logger.LogInformation($"Результат обработки изображения: {output}");
                }

                // Создаем новый слой карты
                return await CreateMapLayerFromTilesAsync(name, description, outputDirectory, isActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обработке изображения карты: {inputImagePath}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<int> RefreshTileMetadataAsync(int layerId)
        {
            try
            {
                // Получаем слой
                var layer = await _unitOfWork.MapLayers.GetByIdAsync(layerId);
                if (layer == null)
                {
                    _logger.LogWarning($"Слой не найден: layerId={layerId}");
                    return 0;
                }

                // Получаем полный путь к директории с тайлами
                var layerPath = Path.Combine(_baseTilesDirectory, layer.BaseDirectory);
                if (!Directory.Exists(layerPath))
                {
                    _logger.LogError($"Директория с тайлами не найдена: {layerPath}");
                    return 0;
                }

                // Счетчик обработанных тайлов
                int processedTiles = 0;

                // Проходим по всем уровням масштабирования
                for (int zoom = layer.MinZoom; zoom <= layer.MaxZoom; zoom++)
                {
                    var zoomPath = Path.Combine(layerPath, zoom.ToString());
                    if (!Directory.Exists(zoomPath))
                    {
                        continue;
                    }

                    // Проходим по всем колонкам (X)
                    foreach (var xPath in Directory.GetDirectories(zoomPath))
                    {
                        if (!int.TryParse(Path.GetFileName(xPath), out int x))
                        {
                            continue;
                        }

                        // Проходим по всем строкам (Y)
                        foreach (var yFile in Directory.GetFiles(xPath, $"*.{layer.TileFormat}"))
                        {
                            var yFileName = Path.GetFileNameWithoutExtension(yFile);
                            if (!int.TryParse(yFileName, out int y))
                            {
                                continue;
                            }

                            // Проверяем, существует ли метаданные для этого тайла
                            var tileMetadata = await _unitOfWork.MapTileMetadata.GetTileMetadataAsync(layerId, zoom, x, y);

                            // Получаем информацию о файле
                            var fileInfo = new FileInfo(yFile);
                            var relativePath = Path.Combine(zoom.ToString(), x.ToString(), $"{y}.{layer.TileFormat}");
                            var checksum = await CalculateChecksumAsync(yFile);

                            if (tileMetadata == null)
                            {
                                // Создаем новые метаданные
                                tileMetadata = new MapTileMetadata
                                {
                                    MapLayerId = layerId,
                                    ZoomLevel = zoom,
                                    TileColumn = x,
                                    TileRow = y,
                                    FileSize = (int)fileInfo.Length,
                                    Checksum = checksum,
                                    RelativePath = relativePath,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                };

                                await _unitOfWork.MapTileMetadata.AddAsync(tileMetadata);
                            }
                            else
                            {
                                // Обновляем существующие метаданные
                                tileMetadata.FileSize = (int)fileInfo.Length;
                                tileMetadata.Checksum = checksum;
                                tileMetadata.RelativePath = relativePath;
                                tileMetadata.UpdatedAt = DateTime.UtcNow;

                                _unitOfWork.MapTileMetadata.Update(tileMetadata);
                            }

                            processedTiles++;
                        }
                    }
                }

                // Сохраняем изменения
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Обновлены метаданные для {processedTiles} тайлов слоя '{layer.Name}'");
                return processedTiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении метаданных тайлов: layerId={layerId}");
                return 0;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MapLayer>> GetAllActiveLayersAsync()
        {
            return await _unitOfWork.MapLayers.GetAllActiveLayersAsync();
        }
        
        /// <inheritdoc/>
        public async Task<MapLayer> GetLayerByIdAsync(int id)
        {
            return await _unitOfWork.MapLayers.GetByIdAsync(id);
        }
        
        /// <inheritdoc/>
        public async Task<MapTileMetadata> GetTileMetadataAsync(int layerId, int zoom, int x, int y)
        {
            return await _unitOfWork.MapTileMetadata.GetTileMetadataAsync(layerId, zoom, x, y);
        }

        /// <summary>
        /// Вычисляет контрольную сумму файла
        /// </summary>
        private async Task<string> CalculateChecksumAsync(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = await md5.ComputeHashAsync(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
} 