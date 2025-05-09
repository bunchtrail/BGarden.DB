using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    /// <summary>
    /// Сервис для управления изображениями образцов
    /// </summary>
    public class SpecimenImageService : ISpecimenImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SpecimenImageService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _imageBasePath = "specimen-images";

        public SpecimenImageService(
            IUnitOfWork unitOfWork,
            ILogger<SpecimenImageService> logger,
            IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<SpecimenImageDto>> GetBySpecimenIdAsync(int specimenId, bool includeImageData = false)
        {
            try
            {
                var images = await _unitOfWork.SpecimenImages.GetBySpecimenIdAsync(specimenId);
                return images.Select(img => img.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображений для образца с ID {SpecimenId}", specimenId);
                throw;
            }
        }

        public async Task<SpecimenImageDto?> GetMainImageBySpecimenIdAsync(int specimenId, bool includeImageData = true)
        {
            try
            {
                var image = await _unitOfWork.SpecimenImages.GetMainImageBySpecimenIdAsync(specimenId);
                if (image == null) return null;
                return image.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении основного изображения для образца с ID {SpecimenId}", specimenId);
                throw;
            }
        }

        public async Task<SpecimenImageDto?> GetByIdAsync(int id, bool includeImageData = true)
        {
            try
            {
                var image = await _unitOfWork.SpecimenImages.GetByIdAsync(id);
                if (image == null) return null;
                return image.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображения с ID {ImageId}", id);
                throw;
            }
        }

        public async Task<SpecimenImageDto> UploadAndAddImageAsync(int specimenId, IFormFile imageFile, string? description, bool isMain)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Файл изображения не предоставлен или пуст.", nameof(imageFile));

            var originalFileName = Path.GetFileName(imageFile.FileName);
            var fileExtension = Path.GetExtension(originalFileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var specimenImageDirectoryRelative = Path.Combine(_imageBasePath, specimenId.ToString("D5"));
            var physicalDirectory = Path.Combine(_hostingEnvironment.WebRootPath, specimenImageDirectoryRelative);
            
            if (!Directory.Exists(physicalDirectory))
            {
                Directory.CreateDirectory(physicalDirectory);
            }

            var relativeFilePath = Path.Combine(specimenImageDirectoryRelative, uniqueFileName).Replace(Path.DirectorySeparatorChar, '/');
            var physicalFilePath = Path.Combine(physicalDirectory, uniqueFileName);

            try
            {
                using (var stream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                _logger.LogInformation("Файл '{OriginalFileName}' сохранен как '{PhysicalFilePath}'", originalFileName, physicalFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении файла '{OriginalFileName}' в '{PhysicalFilePath}'", originalFileName, physicalFilePath);
                throw new IOException($"Ошибка при сохранении файла '{originalFileName}': {ex.Message}", ex);
            }

            var createDto = new CreateSpecimenImageDto
            {
                SpecimenId = specimenId,
                FilePath = relativeFilePath,
                OriginalFileName = originalFileName,
                FileSize = imageFile.Length,
                ContentType = imageFile.ContentType,
                Description = description ?? originalFileName,
                IsMain = isMain
            };

            return await AddAsync(createDto);
        }

        public async Task<SpecimenImageDto> AddAsync(CreateSpecimenImageDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FilePath))
                {
                    throw new ArgumentException("FilePath должен быть указан.", nameof(dto.FilePath));
                }

                var entity = dto.ToEntity();
                entity.UploadedAt = DateTime.UtcNow;

                var addedEntity = await _unitOfWork.SpecimenImages.AddAsync(entity);
                // SaveChangesAsync will be called at a higher level

                return addedEntity.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении записи об изображении для образца с ID {SpecimenId}", dto.SpecimenId);
                throw;
            }
        }

        public async Task<SpecimenImageDto> AddBinaryAsync(CreateSpecimenImageBinaryDto dto)
        {
            _logger.LogInformation("AddBinaryAsync вызван для SpecimenId: {SpecimenId}", dto.SpecimenId);
            if (dto.ImageData == null || dto.ImageData.Length == 0)
                throw new ArgumentException("Данные изображения не предоставлены.", nameof(dto.ImageData));

            var originalFileName = $"binary_{Guid.NewGuid()}"; 
            string fileExtension = dto.ContentType switch {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/webp" => ".webp",
                _ => "" 
            };

            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.Contains('.'))
            {
                 var extFromDesc = Path.GetExtension(dto.Description);
                 if(!string.IsNullOrEmpty(extFromDesc) && (extFromDesc.Length > 1 && extFromDesc.Length <=5))
                 {
                    fileExtension = extFromDesc;
                    originalFileName = Path.GetFileNameWithoutExtension(dto.Description);
                 } else {
                     originalFileName = dto.Description; 
                 }
            }
            if (string.IsNullOrEmpty(fileExtension)) fileExtension = ".img";

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var specimenImageDirectoryRelative = Path.Combine(_imageBasePath, dto.SpecimenId.ToString("D5"));
            var physicalDirectory = Path.Combine(_hostingEnvironment.WebRootPath, specimenImageDirectoryRelative);
            
            if (!Directory.Exists(physicalDirectory))
            {
                Directory.CreateDirectory(physicalDirectory);
            }

            var relativeFilePath = Path.Combine(specimenImageDirectoryRelative, uniqueFileName).Replace(Path.DirectorySeparatorChar, '/');
            var physicalFilePath = Path.Combine(physicalDirectory, uniqueFileName);

            try
            {
                await File.WriteAllBytesAsync(physicalFilePath, dto.ImageData);
                _logger.LogInformation("Бинарный файл сохранен как '{PhysicalFilePath}'", physicalFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении бинарного файла в '{PhysicalFilePath}'", physicalFilePath);
                throw new IOException($"Ошибка при сохранении бинарного файла: {ex.Message}", ex);
            }

            var createDto = new CreateSpecimenImageDto
            {
                SpecimenId = dto.SpecimenId,
                FilePath = relativeFilePath,
                OriginalFileName = string.IsNullOrWhiteSpace(Path.GetExtension(originalFileName)) ? originalFileName + fileExtension : originalFileName,
                FileSize = dto.ImageData.Length,
                ContentType = dto.ContentType,
                Description = dto.Description,
                IsMain = dto.IsMain
            };

            return await AddAsync(createDto);
        }

        public async Task<IEnumerable<SpecimenImageDto>> AddMultipleAsync(IEnumerable<CreateSpecimenImageBinaryDto> dtos)
        {
            var results = new List<SpecimenImageDto>();
            if (dtos == null) return results;

            foreach (var dto in dtos)
            {
                if (dto != null)
                {
                    results.Add(await AddBinaryAsync(dto));
                }
            }
            return results;
        }

        public async Task<SpecimenImageDto?> UpdateAsync(int id, UpdateSpecimenImageDto dto)
        {
             try
            {
                var entity = await _unitOfWork.SpecimenImages.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Изображение с ID {ImageId} не найдено для обновления.", id);
                    return null;
                }

                bool oldIsMain = entity.IsMain;
                entity.ApplyUpdate(dto);
                
                if (entity.IsMain && !oldIsMain)
                {
                    await _unitOfWork.SpecimenImages.UpdateAsync(entity); // Update current first

                    var otherImages = (await _unitOfWork.SpecimenImages.GetBySpecimenIdAsync(entity.SpecimenId))
                                      .Where(oi => oi.Id != entity.Id && oi.IsMain);
                                      
                    foreach (var otherImage in otherImages)
                    {
                        otherImage.IsMain = false;
                        await _unitOfWork.SpecimenImages.UpdateAsync(otherImage);
                    }
                }
                else
                {
                    await _unitOfWork.SpecimenImages.UpdateAsync(entity);
                }
                
                // SaveChangesAsync will be called at a higher level

                return entity.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении изображения с ID {ImageId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var imageToDelete = await _unitOfWork.SpecimenImages.GetByIdAsync(id);
            if (imageToDelete == null)
            {
                _logger.LogWarning("Изображение с ID {ImageId} не найдено для удаления.", id);
                return false;
            }

            var filePathToDelete = imageToDelete.FilePath;

            try
            {
                await _unitOfWork.SpecimenImages.DeleteAsync(id);
                _logger.LogInformation("Запись об изображении с ID {ImageId} помечена к удалению из БД.", id);

                if (imageToDelete.IsMain)
                {
                    var remainingImages = (await _unitOfWork.SpecimenImages.GetBySpecimenIdAsync(imageToDelete.SpecimenId))
                                          .Where(img => img.Id != id)
                                          .OrderByDescending(img => img.UploadedAt)
                                          .ToList();
                    
                    if (remainingImages.Any())
                    {
                        var newMainImage = remainingImages.First();
                        newMainImage.IsMain = true;
                        await _unitOfWork.SpecimenImages.UpdateAsync(newMainImage);
                        _logger.LogInformation("Изображение ID {NewMainImageId} установлено как основное для образца ID {SpecimenId} после удаления старого основного.", newMainImage.Id, imageToDelete.SpecimenId);
                    }
                }

                // File deletion logic (ideally after UoW commit)
                if (!string.IsNullOrWhiteSpace(filePathToDelete))
                {
                    var relativePath = filePathToDelete.TrimStart('/', '\\'); // Corrected TrimStart
                    var physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, relativePath);

                    try
                    {
                        if (File.Exists(physicalPath))
                        {
                            File.Delete(physicalPath);
                            _logger.LogInformation("Файл '{PhysicalPath}' успешно удален.", physicalPath);
                        }
                        else
                        {
                            _logger.LogWarning("Файл '{PhysicalPath}' не найден для удаления.", physicalPath);
                        }
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "Ошибка при удалении файла '{PhysicalPath}'.", physicalPath);
                        // Decide on error handling strategy for file deletion failure
                    }
                }
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при удалении изображения с ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> SetAsMainAsync(int imageId)
        {
            try
            {
                bool result = await _unitOfWork.SpecimenImages.SetAsMainAsync(imageId);
                // SaveChangesAsync will be called at a higher level
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при установке изображения с ID {ImageId} как основного", imageId);
                throw;
            }
        }
    }
} 
