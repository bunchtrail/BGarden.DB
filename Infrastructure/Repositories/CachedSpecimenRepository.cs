using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Application.Mappers;
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
            
            // Инвалидируем общий кеш всех образцов
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
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
                
                // Проверяем изменение координат для инвалидации кэша местоположения
                bool locationChanged = oldEntity.Latitude != entity.Latitude || 
                                      oldEntity.Longitude != entity.Longitude ||
                                      oldEntity.LocationType != entity.LocationType ||
                                      oldEntity.MapId != entity.MapId ||
                                      oldEntity.MapX != entity.MapX ||
                                      oldEntity.MapY != entity.MapY;
                                      
                if (locationChanged)
                {
                    // Инвалидируем кэш для пространственных запросов и фильтров с координатами
                    _logger.LogInformation($"Обнаружено изменение координат у образца {entity.Id}, инвалидация пространственных кэшей");
                    
                    // Инвалидируем кэш для фильтрованных запросов, которые могут включать этот образец
                    InvalidateAllFilteredCaches();
                    
                    // Инвалидируем кэш для запросов в текущем регионе
                    if (entity.RegionId.HasValue)
                        _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{entity.RegionId.Value}").GetAwaiter().GetResult();
                }
            }
            
            _cacheService.RemoveAsync($"{CacheKeyPrefix}InventoryNumber_{entity.InventoryNumber}").GetAwaiter().GetResult();
            
            // Инвалидируем общий кэш
            _cacheService.RemoveAsync($"{CacheKeyPrefix}All").GetAwaiter().GetResult();
            
            // Гарантируем, что кэш конкретной сущности полностью сброшен
            _cacheService.RemoveAsync($"{CacheKeyPrefix}{entity.Id}").GetAwaiter().GetResult();
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
        
        public async Task<IEnumerable<Specimen>> GetFilteredSpecimensAsync(string? name = null, int? familyId = null, int? regionId = null)
        {
            // Для запросов с фильтрацией создаём уникальный ключ кэша на основе параметров
            string cacheKey = $"{CacheKeyPrefix}Filtered_Name_{name ?? "null"}_FamilyId_{familyId ?? 0}_RegionId_{regionId ?? 0}";
            
            return await _cacheService.GetOrCreateAsync(cacheKey, 
                async () => await _specimenRepository.GetFilteredSpecimensAsync(name, familyId, regionId),
                DefaultCacheTime);
        }

        // Добавляем новый метод для инвалидации всех фильтрованных кэшей
        private void InvalidateAllFilteredCaches()
        {
            // Используем новый метод для удаления кэшей по шаблону
            _logger.LogInformation("Инвалидация всех фильтрованных кэшей для обеспечения согласованности данных");
            
            // Удаляем все кэши, содержащие "Filtered"
            _cacheService.RemoveByPatternAsync($"{CacheKeyPrefix}Filtered").GetAwaiter().GetResult();
            
            // Удаляем все кэши, связанные с координатами
            _cacheService.RemoveByPatternAsync("Location").GetAwaiter().GetResult();
            _cacheService.RemoveByPatternAsync("BoundingBox").GetAwaiter().GetResult();
            
            // Инвалидируем общий кэш также
            _cacheService.RemoveAsync($"{CacheKeyPrefix}All").GetAwaiter().GetResult();
        }

        // Переопределим метод UpdateSpecimenLocationAsync для прямой работы с репозиторием
        // и явной инвалидации связанных кэшей
        public async Task<SpecimenDto?> UpdateSpecimenLocationAsync(int id, SpecimenDto locationDto)
        {
            // Получаем существующий образец
            var specimen = await _specimenRepository.GetByIdAsync(id);
            if (specimen == null)
            {
                _logger.LogWarning("Образец с ID {Id} не найден при попытке обновления местоположения", id);
                return null;
            }
            
            // Обновляем координаты в зависимости от выбранного типа
            switch (locationDto.LocationType)
            {
                case BGarden.Domain.Enums.LocationType.Geographic:
                    if (!locationDto.Latitude.HasValue || !locationDto.Longitude.HasValue)
                    {
                        _logger.LogError("При использовании географических координат должны быть указаны широта и долгота");
                        throw new ArgumentException("При использовании географических координат должны быть указаны широта и долгота");
                    }
                    specimen.SetGeographicCoordinates(locationDto.Latitude.Value, locationDto.Longitude.Value);
                    break;
                    
                case BGarden.Domain.Enums.LocationType.SchematicMap:
                    if (!locationDto.MapId.HasValue || !locationDto.MapX.HasValue || !locationDto.MapY.HasValue)
                    {
                        _logger.LogError("При использовании схематической карты должны быть указаны идентификатор карты и координаты X, Y");
                        throw new ArgumentException("При использовании схематической карты должны быть указаны идентификатор карты и координаты X, Y");
                    }
                    specimen.SetSchematicCoordinates(locationDto.MapId.Value, locationDto.MapX.Value, locationDto.MapY.Value);
                    break;
                    
                case BGarden.Domain.Enums.LocationType.None:
                    specimen.ClearCoordinates();
                    break;
            }
            
            specimen.LastUpdatedAt = DateTime.UtcNow;
            
            // Обновляем объект в репозитории
            _specimenRepository.Update(specimen);
            
            try
            {
                // Получаем текущий UnitOfWork через статический метод
                var unitOfWork = UnitOfWork.GetUnitOfWork();
                if (unitOfWork != null)
                {
                    await unitOfWork.SaveChangesAsync();
                    _logger.LogInformation("Изменения местоположения сохранены в базе данных для образца {Id}", id);
                }
                else
                {
                    _logger.LogWarning("Не удалось получить UnitOfWork для сохранения изменений местоположения образца {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении изменений местоположения для образца {Id}", id);
            }
            
            // Явная инвалидация всех связанных кэшей при обновлении местоположения
            _logger.LogInformation("Явная инвалидация кэшей при обновлении местоположения для образца {SpecimenId}", id);
            
            // Удаляем кэш конкретного образца
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}WithDetails_{id}");
            
            // Удаляем все кэши с фильтрацией
            await _cacheService.RemoveByPatternAsync($"{CacheKeyPrefix}Filtered");
            
            // Удаляем кэши, связанные с пространственными запросами
            await _cacheService.RemoveByPatternAsync("BoundingBox");
            
            // Удаляем кэш для региона, если он указан
            if (specimen.RegionId.HasValue)
            {
                await _cacheService.RemoveAsync($"{CacheKeyPrefix}ByRegion_{specimen.RegionId.Value}");
            }
            
            // Удаляем общий кэш
            await _cacheService.RemoveAsync($"{CacheKeyPrefix}All");
            
            // Возвращаем DTO
            return SpecimenMapper.ToDto(specimen);
        }
    }
} 