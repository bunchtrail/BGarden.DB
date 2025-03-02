using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Entities;
using BGarden.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Сервис, реализующий бизнес-операции с Phenology.
    /// </summary>
    public class PhenologyService : IPhenologyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhenologyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PhenologyDto>> GetAllPhenologiesAsync()
        {
            var phenologies = await _unitOfWork.Phenologies.GetAllAsync();
            return phenologies.Select(p => p.ToDto()).ToList();
        }

        public async Task<PhenologyDto?> GetPhenologyByIdAsync(int id)
        {
            var phenology = await _unitOfWork.Phenologies.GetByIdAsync(id);
            return phenology?.ToDto();
        }
        
        public async Task<IEnumerable<PhenologyDto>> GetPhenologiesBySpecimenIdAsync(int specimenId)
        {
            var phenologies = await _unitOfWork.Phenologies.GetBySpecimenIdAsync(specimenId);
            return phenologies.Select(p => p.ToDto()).ToList();
        }
        
        public async Task<IEnumerable<PhenologyDto>> GetPhenologiesByYearAsync(int year)
        {
            var phenologies = await _unitOfWork.Phenologies.GetByYearAsync(year);
            return phenologies.Select(p => p.ToDto()).ToList();
        }
        
        public async Task<PhenologyDto?> GetPhenologyBySpecimenAndYearAsync(int specimenId, int year)
        {
            var phenology = await _unitOfWork.Phenologies.GetBySpecimenAndYearAsync(specimenId, year);
            return phenology?.ToDto();
        }
        
        public async Task<IEnumerable<PhenologyDto>> GetPhenologiesByFloweringPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var phenologies = await _unitOfWork.Phenologies.GetByFloweringPeriodAsync(startDate, endDate);
            return phenologies.Select(p => p.ToDto()).ToList();
        }

        public async Task<PhenologyDto> CreatePhenologyAsync(PhenologyDto phenologyDto)
        {
            var entity = phenologyDto.ToEntity();
            await _unitOfWork.Phenologies.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<PhenologyDto?> UpdatePhenologyAsync(int id, PhenologyDto phenologyDto)
        {
            var existing = await _unitOfWork.Phenologies.GetByIdAsync(id);
            if (existing == null) return null;

            phenologyDto.UpdateEntity(existing);
            _unitOfWork.Phenologies.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeletePhenologyAsync(int id)
        {
            var existing = await _unitOfWork.Phenologies.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Phenologies.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
} 