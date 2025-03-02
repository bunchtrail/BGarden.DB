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
    /// Сервис, реализующий бизнес-операции с Biometry.
    /// </summary>
    public class BiometryService : IBiometryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BiometryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BiometryDto>> GetAllBiometriesAsync()
        {
            var biometries = await _unitOfWork.Biometries.GetAllAsync();
            return biometries.Select(b => b.ToDto()).ToList();
        }

        public async Task<BiometryDto?> GetBiometryByIdAsync(int id)
        {
            var biometry = await _unitOfWork.Biometries.GetByIdAsync(id);
            return biometry?.ToDto();
        }
        
        public async Task<IEnumerable<BiometryDto>> GetBiometriesBySpecimenIdAsync(int specimenId)
        {
            var biometries = await _unitOfWork.Biometries.GetBySpecimenIdAsync(specimenId);
            return biometries.Select(b => b.ToDto()).ToList();
        }
        
        public async Task<IEnumerable<BiometryDto>> GetBiometriesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var biometries = await _unitOfWork.Biometries.GetByDateRangeAsync(startDate, endDate);
            return biometries.Select(b => b.ToDto()).ToList();
        }
        
        public async Task<IEnumerable<BiometryDto>> GetLatestBiometriesForSpecimenAsync(int specimenId, int count = 1)
        {
            var biometries = await _unitOfWork.Biometries.GetLatestForSpecimenAsync(specimenId, count);
            return biometries.Select(b => b.ToDto()).ToList();
        }

        public async Task<BiometryDto> CreateBiometryAsync(BiometryDto biometryDto)
        {
            var entity = biometryDto.ToEntity();
            await _unitOfWork.Biometries.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<BiometryDto?> UpdateBiometryAsync(int id, BiometryDto biometryDto)
        {
            var existing = await _unitOfWork.Biometries.GetByIdAsync(id);
            if (existing == null) return null;

            biometryDto.UpdateEntity(existing);
            _unitOfWork.Biometries.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeleteBiometryAsync(int id)
        {
            var existing = await _unitOfWork.Biometries.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Biometries.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
} 