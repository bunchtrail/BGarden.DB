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
    /// Реализация сервиса для работы с Region (областями).
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync()
        {
            var regions = await _unitOfWork.Regions.GetAllRegionsAsync();
            return regions.ToDto();
        }

        /// <inheritdoc/>
        public async Task<RegionDto?> GetRegionByIdAsync(int id)
        {
            var region = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            return region?.ToDto();
        }

        /// <inheritdoc/>
        public async Task<RegionDto> CreateRegionAsync(RegionDto regionDto)
        {
            var region = regionDto.ToEntity();
            
            await _unitOfWork.Regions.AddAsync(region);
            await _unitOfWork.SaveChangesAsync();
            
            return region.ToDto();
        }

        /// <inheritdoc/>
        public async Task<RegionDto?> UpdateRegionAsync(int id, RegionDto regionDto)
        {
            var existingRegion = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            if (existingRegion == null)
                return null;

            existingRegion.UpdateFromDto(regionDto);
            
            _unitOfWork.Regions.Update(existingRegion);
            await _unitOfWork.SaveChangesAsync();
            
            return existingRegion.ToDto();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteRegionAsync(int id)
        {
            var region = await _unitOfWork.Regions.GetRegionByIdAsync(id);
            if (region == null)
                return false;

            _unitOfWork.Regions.Remove(region);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SpecimenDto>> GetSpecimensInRegionAsync(int regionId)
        {
            var region = await _unitOfWork.Regions.GetRegionByIdAsync(regionId);
            if (region == null)
                return Enumerable.Empty<SpecimenDto>();

            // Получаем образцы из данного региона через репозиторий Specimen
            var specimens = await _unitOfWork.Specimens.GetSpecimensByRegionIdAsync(regionId);
            return specimens.Select(s => s.ToDto());
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
    }
} 