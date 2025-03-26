using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с изображениями образцов
    /// </summary>
    public class SpecimenImageRepository : ISpecimenImageRepository
    {
        private readonly BotanicalContext _context;
        private readonly ILogger<SpecimenImageRepository> _logger;

        public SpecimenImageRepository(BotanicalContext context, ILogger<SpecimenImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<SpecimenImage>> GetBySpecimenIdAsync(int specimenId)
        {
            try
            {
                return await _context.SpecimenImages
                    .Where(si => si.SpecimenId == specimenId)
                    .OrderByDescending(si => si.IsMain)
                    .ThenByDescending(si => si.UploadedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображений для образца с ID {SpecimenId}", specimenId);
                return Enumerable.Empty<SpecimenImage>();
            }
        }

        public async Task<SpecimenImage?> GetMainImageBySpecimenIdAsync(int specimenId)
        {
            try
            {
                return await _context.SpecimenImages
                    .Where(si => si.SpecimenId == specimenId && si.IsMain)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении основного изображения для образца с ID {SpecimenId}", specimenId);
                return null;
            }
        }

        public async Task<SpecimenImage?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.SpecimenImages.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображения с ID {ImageId}", id);
                return null;
            }
        }

        public async Task<SpecimenImage> AddAsync(SpecimenImage image)
        {
            try
            {
                // Если изображение помечено как основное, снимаем этот флаг с других изображений для того же образца
                if (image.IsMain)
                {
                    var existingMainImages = await _context.SpecimenImages
                        .Where(si => si.SpecimenId == image.SpecimenId && si.IsMain)
                        .ToListAsync();
                        
                    foreach (var mainImage in existingMainImages)
                    {
                        mainImage.IsMain = false;
                    }
                }

                // Устанавливаем дату загрузки
                image.UploadedAt = DateTime.UtcNow;
                
                await _context.SpecimenImages.AddAsync(image);
                await _context.SaveChangesAsync();
                
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении изображения для образца с ID {SpecimenId}", image.SpecimenId);
                throw;
            }
        }

        public async Task<SpecimenImage> UpdateAsync(SpecimenImage image)
        {
            try
            {
                // Если изображение помечено как основное, снимаем этот флаг с других изображений для того же образца
                if (image.IsMain)
                {
                    var existingMainImages = await _context.SpecimenImages
                        .Where(si => si.SpecimenId == image.SpecimenId && si.IsMain && si.Id != image.Id)
                        .ToListAsync();
                        
                    foreach (var mainImage in existingMainImages)
                    {
                        mainImage.IsMain = false;
                    }
                }
                
                _context.SpecimenImages.Update(image);
                await _context.SaveChangesAsync();
                
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении изображения с ID {ImageId}", image.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var image = await _context.SpecimenImages.FindAsync(id);
                if (image == null)
                {
                    return false;
                }
                
                _context.SpecimenImages.Remove(image);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении изображения с ID {ImageId}", id);
                return false;
            }
        }

        public async Task<bool> SetAsMainAsync(int imageId)
        {
            try
            {
                var image = await _context.SpecimenImages.FindAsync(imageId);
                if (image == null)
                {
                    return false;
                }
                
                // Снимаем флаг основного изображения с других изображений для этого образца
                var existingMainImages = await _context.SpecimenImages
                    .Where(si => si.SpecimenId == image.SpecimenId && si.IsMain && si.Id != imageId)
                    .ToListAsync();
                    
                foreach (var mainImage in existingMainImages)
                {
                    mainImage.IsMain = false;
                }
                
                // Устанавливаем текущее изображение как основное
                image.IsMain = true;
                
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при установке изображения с ID {ImageId} как основного", imageId);
                return false;
            }
        }
    }
} 