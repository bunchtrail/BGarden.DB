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
    /// Сервис, реализующий бизнес-операции с Family.
    /// </summary>
    public class FamilyService : IFamilyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FamilyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync()
        {
            var families = await _unitOfWork.Families.GetAllAsync();
            return families.Select(f => f.ToDto()).ToList();
        }

        public async Task<FamilyDto?> GetFamilyByIdAsync(int id)
        {
            var family = await _unitOfWork.Families.GetByIdAsync(id);
            return family?.ToDto();
        }
        
        public async Task<FamilyDto?> GetFamilyByNameAsync(string name)
        {
            var family = await _unitOfWork.Families.FindByNameAsync(name);
            return family?.ToDto();
        }

        public async Task<FamilyDto> CreateFamilyAsync(FamilyDto familyDto)
        {
            var entity = familyDto.ToEntity();
            await _unitOfWork.Families.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<FamilyDto?> UpdateFamilyAsync(int id, FamilyDto familyDto)
        {
            var existing = await _unitOfWork.Families.GetByIdAsync(id);
            if (existing == null) return null;

            familyDto.UpdateEntity(existing);
            _unitOfWork.Families.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeleteFamilyAsync(int id)
        {
            var existing = await _unitOfWork.Families.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Families.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
} 