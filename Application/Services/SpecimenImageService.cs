using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    /// <summary>
    /// Сервис для управления изображениями образцов
    /// </summary>
    public class SpecimenImageService : ISpecimenImageService
    {
        private readonly ISpecimenImageRepository _repository;
        private readonly ILogger<SpecimenImageService> _logger;

        public SpecimenImageService(
            ISpecimenImageRepository repository,
            ILogger<SpecimenImageService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<SpecimenImageDto>> GetBySpecimenIdAsync(int specimenId, bool includeImageData = false)
        {
            var images = await _repository.GetBySpecimenIdAsync(specimenId);
            return images.Select(img => img.ToDto(includeImageData)).ToList();
        }

        public async Task<SpecimenImageDto?> GetMainImageBySpecimenIdAsync(int specimenId, bool includeImageData = true)
        {
            var image = await _repository.GetMainImageBySpecimenIdAsync(specimenId);
            return image?.ToDto(includeImageData);
        }

        public async Task<SpecimenImageDto?> GetByIdAsync(int id, bool includeImageData = true)
        {
            var image = await _repository.GetByIdAsync(id);
            return image?.ToDto(includeImageData);
        }

        public async Task<SpecimenImageDto> AddAsync(CreateSpecimenImageDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                var result = await _repository.AddAsync(entity);
                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении изображения для образца с ID {SpecimenId}", dto.SpecimenId);
                throw;
            }
        }
            
        public async Task<SpecimenImageDto> AddBinaryAsync(CreateSpecimenImageBinaryDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                var result = await _repository.AddAsync(entity);
                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении бинарного изображения для образца с ID {SpecimenId}", dto.SpecimenId);
                throw;
            }
        }
        
        public async Task<IEnumerable<SpecimenImageDto>> AddMultipleAsync(IEnumerable<CreateSpecimenImageBinaryDto> dtos)
        {
            try
            {
                var results = new List<SpecimenImageDto>();
                
                foreach (var dto in dtos)
                {
                    var entity = dto.ToEntity();
                    var result = await _repository.AddAsync(entity);
                    results.Add(result.ToDto());
                }
                
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при массовой загрузке изображений");
                throw;
            }
        }

        public async Task<SpecimenImageDto?> UpdateAsync(int id, UpdateSpecimenImageDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                entity = entity.ApplyUpdate(dto);
                var result = await _repository.UpdateAsync(entity);
                return result.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении изображения с ID {ImageId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> SetAsMainAsync(int imageId)
        {
            return await _repository.SetAsMainAsync(imageId);
        }
    }
} 