using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using BGarden.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

namespace BGarden.Infrastructure.Repositories
{
    /// <summary>
    /// Оптимизированная реализация репозитория образцов с кэшированием
    /// </summary>
    public class CachedSpecimenRepository : LoggingRepositoryDecorator<Specimen>, ISpecimenRepository
    {
        private readonly ISpecimenRepository _specimenRepository;
        private readonly ICacheService _cacheService;
        private new readonly ILogger<CachedSpecimenRepository> _logger;

        private const string CacheKeyPrefix = "Specimen_";
        private static readonly TimeSpan DefaultCacheTime = TimeSpan.FromMinutes(15);

        public CachedSpecimenRepository(
            ISpecimenRepository specimenRepository,
            ICacheService cacheService,
            ILogger<CachedSpecimenRepository> logger)
            : base(specimenRepository, logger)
        {
            _specimenRepository = specimenRepository ?? throw new ArgumentNullException(nameof(specimenRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Specimen?> FindByInventoryNumberAsync(string inventoryNumber)
        {
            string cacheKey = $"{CacheKeyPrefix}InventoryNumber_{inventoryNumber}";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await _specimenRepository.FindByInventoryNumberAsync(inventoryNumber),
                DefaultCacheTime);
        }

        public async Task<IEnumerable<Specimen>> FindBySpeciesNameAsync(string speciesName)
        {
            // Для этого метода кэширование менее эффективно, так как запрос поисковый
            // и нет гарантии, что одни и те же запросы будут повторяться часто
            return await _specimenRepository.FindBySpeciesNameAsync(speciesName);
        }

        public async Task<IEnumerable<Specimen>> GetSpecimensByRegionIdAsync(int regionId)
        {
            string cacheKey = $"{CacheKeyPrefix}ByRegion_{regionId}";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await _specimenRepository.GetSpecimensByRegionIdAsync(regionId),
                DefaultCacheTime);
        }

        public async Task<IEnumerable<Specimen>> GetSpecimensBySectorTypeAsync(SectorType sectorType)
        {
            string cacheKey = $"{CacheKeyPrefix}BySectorType_{sectorType}";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await _specimenRepository.GetSpecimensBySectorTypeAsync(sectorType),
                DefaultCacheTime);
        }

        public async Task<Specimen?> GetByIdWithDetailsAsync(int id)
        {
            string cacheKey = $"{CacheKeyPrefix}WithDetails_{id}";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await _specimenRepository.GetByIdWithDetailsAsync(id),
                DefaultCacheTime);
        }

        /// <summary>
        /// Переопределяем стандартные методы базового класса для очистки кэша при изменениях
        /// </summary>
        public override async Task AddAsync(Specimen entity)
        {
            await base.AddAsync(entity);
            
            if (entity.RegionId.HasValue)
                await InvalidateRegionCacheAsync(entity.RegionId.Value);
                
            await InvalidateSectorTypeCacheAsync(entity.SectorType);
        }

        public override void Update(Specimen entity)
        {
            // Получаем старую версию сущности, чтобы инвалидировать соответствующие кэши
            var oldEntity = _repository.GetByIdAsync(entity.Id).GetAwaiter().GetResult();
            
            base.Update(entity);
            
            // Инвалидируем кэш для сущности
            _cacheService.RemoveAsync($"{CacheKeyPrefix}WithDetails_{entity.Id}").GetAwaiter().GetResult();
            
            // Инвалидируем кэши для связанных сущностей, если они изменились
            if (oldEntity != null)
            {
                if (oldEntity.RegionId != entity.RegionId)
                {
                    if (oldEntity.RegionId.HasValue)
                        _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{oldEntity.RegionId.Value}").GetAwaiter().GetResult();
                    if (entity.RegionId.HasValue)
                        _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{entity.RegionId.Value}").GetAwaiter().GetResult();
                }
                
                if (oldEntity.SectorType != entity.SectorType)
                {
                    _cacheService.RemoveAsync($"{CacheKeyPrefix}BySectorType_{oldEntity.SectorType}").GetAwaiter().GetResult();
                    _cacheService.RemoveAsync($"{CacheKeyPrefix}BySectorType_{entity.SectorType}").GetAwaiter().GetResult();
                }
                
                if (oldEntity.InventoryNumber != entity.InventoryNumber)
                {
                    _cacheService.RemoveAsync($"{CacheKeyPrefix}InventoryNumber_{oldEntity.InventoryNumber}").GetAwaiter().GetResult();
                }
            }
            
            _cacheService.RemoveAsync($"{CacheKeyPrefix}InventoryNumber_{entity.InventoryNumber}").GetAwaiter().GetResult();
            
            // Инвалидируем общий кэш
            _cacheService.RemoveAsync($"{CacheKeyPrefix}All").GetAwaiter().GetResult();
        }

        public override void Remove(Specimen entity)
        {
            base.Remove(entity);
            
            // Инвалидируем все связанные кэши
            _cacheService.RemoveAsync($"{CacheKeyPrefix}WithDetails_{entity.Id}").GetAwaiter().GetResult();
            _cacheService.RemoveAsync($"{CacheKeyPrefix}InventoryNumber_{entity.InventoryNumber}").GetAwaiter().GetResult();
            
            if (entity.RegionId.HasValue)
                _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{entity.RegionId.Value}").GetAwaiter().GetResult();
            
            _cacheService.RemoveAsync($"{CacheKeyPrefix}BySectorType_{entity.SectorType}").GetAwaiter().GetResult();
            _cacheService.RemoveAsync($"{CacheKeyPrefix}All").GetAwaiter().GetResult();
        }

        private async Task InvalidateRegionCacheAsync(int regionId)
        {
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{regionId}");
        }

        private async Task InvalidateSectorTypeCacheAsync(SectorType sectorType)
        {
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}BySectorType_{sectorType}");
        }

        // Переопределяем базовый метод для кэширования
        public override async Task<IEnumerable<Specimen>> GetAllAsync()
        {
            string cacheKey = $"{CacheKeyPrefix}All";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await base.GetAllAsync(),
                DefaultCacheTime);
        }

        public async Task<IEnumerable<Specimen>> GetSpecimensInBoundingBoxAsync(Envelope boundingBox)
        {
            // Для пространственных запросов кэширование менее эффективно, так как
            // границы области могут быть разными в каждом запросе
            // Поэтому просто делегируем вызов базовому репозиторию
            return await _specimenRepository.GetSpecimensInBoundingBoxAsync(boundingBox);
        }
    }
} 