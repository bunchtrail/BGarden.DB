using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Сервис, реализующий бизнес-операции с Exposition.
    /// </summary>
    public class ExpositionService : IExpositionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExpositionDto>> GetAllExpositionsAsync()
        {
            var expositions = await _unitOfWork.Expositions.GetAllAsync();
            return expositions.Select(e => e.ToDto()).ToList();
        }

        public async Task<ExpositionDto?> GetExpositionByIdAsync(int id)
        {
            var exposition = await _unitOfWork.Expositions.GetByIdAsync(id);
            return exposition?.ToDto();
        }
        
        public async Task<ExpositionDto?> GetExpositionByNameAsync(string name)
        {
            var exposition = await _unitOfWork.Expositions.FindByNameAsync(name);
            return exposition?.ToDto();
        }

        public async Task<ExpositionDto> CreateExpositionAsync(ExpositionDto expositionDto)
        {
            var entity = expositionDto.ToEntity();
            await _unitOfWork.Expositions.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<ExpositionDto?> UpdateExpositionAsync(int id, ExpositionDto expositionDto)
        {
            var existing = await _unitOfWork.Expositions.GetByIdAsync(id);
            if (existing == null) return null;

            expositionDto.UpdateEntity(existing);
            _unitOfWork.Expositions.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeleteExpositionAsync(int id)
        {
            var existing = await _unitOfWork.Expositions.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Expositions.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
} 