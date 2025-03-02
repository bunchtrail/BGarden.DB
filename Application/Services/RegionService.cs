using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using BGarden.Domain.Interfaces;

namespace Application.Services
{
    /// <summary>
    /// Сервис, реализующий бизнес-операции с регионами
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync()
        {
            var regions = await _unitOfWork.Regions.GetAllRegionsAsync();
            return regions.Select(r => r.ToDto()).ToList();
        }

        public async Task<RegionDto?> GetRegionByIdAsync(int id)
        {
            var region = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            return region?.ToDto();
        }

        public async Task<IEnumerable<RegionDto>> GetRegionsBySectorTypeAsync(SectorType sectorType)
        {
            var regions = await _unitOfWork.Regions.GetRegionsBySectorTypeAsync(sectorType);
            return regions.Select(r => r.ToDto()).ToList();
        }

        public async Task<IEnumerable<RegionDto>> GetRegionsWithSpecimensAsync()
        {
            var regions = await _unitOfWork.Regions.GetRegionsWithSpecimensAsync();
            return regions.Select(r => r.ToDto()).ToList();
        }

        public async Task<IEnumerable<RegionDto>> FindNearbyRegionsAsync(decimal latitude, decimal longitude, decimal radiusInMeters)
        {
            var regions = await _unitOfWork.Regions.FindNearbyRegionsAsync(latitude, longitude, radiusInMeters);
            return regions.Select(r => r.ToDto()).ToList();
        }

        public async Task<RegionDto> CreateRegionAsync(RegionDto regionDto)
        {
            var entity = regionDto.ToEntity();
            await _unitOfWork.Regions.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<RegionDto?> UpdateRegionAsync(int id, RegionDto regionDto)
        {
            var existing = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            if (existing == null) return null;

            regionDto.UpdateEntity(existing);
            _unitOfWork.Regions.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return existing.ToDto();
        }

        public async Task<bool> DeleteRegionAsync(int id)
        {
            var existing = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            if (existing == null) return false;

            // Проверяем, есть ли образцы, связанные с этим регионом
            if (existing.Specimens != null && existing.Specimens.Any())
            {
                // Отвязываем образцы от региона перед удалением
                foreach (var specimen in existing.Specimens)
                {
                    specimen.RegionId = null;
                    specimen.Region = null;
                    _unitOfWork.Specimens.Update(specimen);
                }
            }

            _unitOfWork.Regions.Remove(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SpecimenDto>> GetSpecimensInRegionAsync(int regionId)
        {
            var region = await _unitOfWork.Regions.GetRegionByIdAsync(regionId);
            if (region == null || region.Specimens == null) 
                return Enumerable.Empty<SpecimenDto>();

            return region.Specimens.Select(s => s.ToDto()).ToList();
        }
    }
} 