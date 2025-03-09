using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using BGarden.Domain.Enums;
using NetTopologySuite.Geometries;

namespace Application.Services
{
    /// <summary>
    /// Сервис, реализующий бизнес-операции со Specimen.
    /// Использует репозиторий (через UnitOfWork) и маппит сущности в DTO.
    /// </summary>
    public class SpecimenService : ISpecimenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecimenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SpecimenDto>> GetAllSpecimensAsync()
        {
            var specimens = await _unitOfWork.Specimens.GetAllAsync();
            return specimens.Select(s => s.ToDto()).ToList();
        }

        public async Task<SpecimenDto?> GetSpecimenByIdAsync(int id)
        {
            var specimen = await _unitOfWork.Specimens.GetByIdAsync(id);
            return specimen?.ToDto();
        }

        public async Task<SpecimenDto> CreateSpecimenAsync(SpecimenDto specimenDto)
        {
            // Маппим DTO -> новая сущность
            var entity = specimenDto.ToEntity();
            
            // Добавляем в репозиторий
            await _unitOfWork.Specimens.AddAsync(entity);
            
            // Сохраняем (UnitOfWork)
            await _unitOfWork.SaveChangesAsync();

            // Возвращаем DTO (теперь у entity есть Id)
            return entity.ToDto();
        }

        public async Task<SpecimenDto?> UpdateSpecimenAsync(int id, SpecimenDto specimenDto)
        {
            var existing = await _unitOfWork.Specimens.GetByIdAsync(id);
            if (existing == null) return null;

            // Маппим новые данные из DTO
            specimenDto.UpdateEntity(existing);

            // В репозитории обычно достаточно пометить объект, EF сам отследит изменения
            // Update вызов не всегда обязателен, но можем явно вызвать:
            _unitOfWork.Specimens.Update(existing);

            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeleteSpecimenAsync(int id)
        {
            var existing = await _unitOfWork.Specimens.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Specimens.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SpecimenDto>> GetSpecimensBySectorTypeAsync(SectorType sectorType)
        {
            var specimens = await _unitOfWork.Specimens.GetSpecimensBySectorTypeAsync(sectorType);
            return specimens.Select(s => s.ToDto()).ToList();
        }
        
        public async Task<IEnumerable<SpecimenDto>> GetSpecimensInBoundingBoxAsync(decimal minLat, decimal minLng, decimal maxLat, decimal maxLng)
        {
            // Создаем Envelope (прямоугольник) из координат
            var boundingBox = new Envelope(
                (double)minLng, 
                (double)maxLng, 
                (double)minLat, 
                (double)maxLat
            );
            
            var specimens = await _unitOfWork.Specimens.GetSpecimensInBoundingBoxAsync(boundingBox);
            return specimens.Select(s => s.ToDto()).ToList();
        }
        
        public async Task<SpecimenDto> AddSpecimenToMapAsync(SpecimenDto specimenDto)
        {
            // Проверяем, что координаты указаны
            if (!specimenDto.Latitude.HasValue || !specimenDto.Longitude.HasValue)
            {
                throw new ArgumentException("Для добавления растения на карту необходимо указать координаты (широту и долготу)");
            }
            
            // Используем существующий метод создания
            return await CreateSpecimenAsync(specimenDto);
        }
        
        public async Task<SpecimenDto?> UpdateSpecimenLocationAsync(int id, decimal latitude, decimal longitude)
        {
            var existing = await _unitOfWork.Specimens.GetByIdAsync(id);
            if (existing == null) return null;
            
            // Обновляем только координаты
            existing.Latitude = latitude;
            existing.Longitude = longitude;
            existing.Location = new Point((double)longitude, (double)latitude) { SRID = 4326 };
            existing.LastUpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync();
            
            return existing.ToDto();
        }
    }
} 