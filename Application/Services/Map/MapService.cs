using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Map;
using BGarden.DB.Application.DTO;
using BGarden.DB.Domain.Entities;
using BGarden.DB.Domain.Enums;
using BGarden.DB.Domain.Interfaces;
using BGarden.Domain.Interfaces;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Application.Services.Map
{
    /// <summary>
    /// Реализация сервиса для работы с картой
    /// </summary>
    public class MapService : IMapService
    {
        private readonly IMapMarkerRepository _markerRepository;
        private readonly IMapOptionsRepository _optionsRepository;
        private readonly IMapLayerRepository _layerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly WKTReader _wktReader;
        private readonly WKTWriter _wktWriter;

        public MapService(
            IMapMarkerRepository markerRepository,
            IMapOptionsRepository optionsRepository,
            IMapLayerRepository layerRepository,
            IUnitOfWork unitOfWork)
        {
            _markerRepository = markerRepository ?? throw new ArgumentNullException(nameof(markerRepository));
            _optionsRepository = optionsRepository ?? throw new ArgumentNullException(nameof(optionsRepository));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _wktReader = new WKTReader();
            _wktWriter = new WKTWriter();
        }

        #region MapMarkers

        public async Task<IEnumerable<MapMarkerDto>> GetAllMarkersAsync()
        {
            var markers = await _markerRepository.GetAllAsync();
            return markers.Select(MapMarkerToDto);
        }

        public async Task<MapMarkerDto> GetMarkerByIdAsync(int id)
        {
            var marker = await _markerRepository.GetByIdAsync(id);
            if (marker == null)
            {
                throw new KeyNotFoundException($"Маркер с Id {id} не найден");
            }
            return MapMarkerToDto(marker);
        }

        public async Task<IEnumerable<MapMarkerDto>> GetMarkersByTypeAsync(MarkerType type)
        {
            var markers = await _markerRepository.GetByTypeAsync(type);
            return markers.Select(MapMarkerToDto);
        }

        public async Task<IEnumerable<MapMarkerDto>> GetMarkersBySpecimenIdAsync(int specimenId)
        {
            var marker = await _markerRepository.GetBySpecimenIdAsync(specimenId);
            if (marker == null)
            {
                return Enumerable.Empty<MapMarkerDto>();
            }
            return new List<MapMarkerDto> { MapMarkerToDto(marker) };
        }

        public async Task<MapMarkerDto> CreateMarkerAsync(CreateMapMarkerDto dto)
        {
            var marker = new MapMarker
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Title = dto.Title,
                Type = dto.Type,
                Description = dto.Description,
                PopupContent = dto.PopupContent,
                SpecimenId = dto.SpecimenId ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _markerRepository.AddAsync(marker);
            await _unitOfWork.SaveChangesAsync();

            return MapMarkerToDto(marker);
        }

        public async Task<MapMarkerDto> UpdateMarkerAsync(UpdateMapMarkerDto dto)
        {
            var marker = await _markerRepository.GetByIdAsync(dto.Id);
            if (marker == null)
            {
                throw new KeyNotFoundException($"Маркер с Id {dto.Id} не найден");
            }

            marker.Latitude = dto.Latitude;
            marker.Longitude = dto.Longitude;
            marker.Title = dto.Title;
            marker.Type = dto.Type;
            marker.Description = dto.Description;
            marker.PopupContent = dto.PopupContent;
            marker.SpecimenId = dto.SpecimenId ?? 0;
            marker.UpdatedAt = DateTime.UtcNow;

            _markerRepository.Update(marker);
            await _unitOfWork.SaveChangesAsync();

            return MapMarkerToDto(marker);
        }

        public async Task<bool> DeleteMarkerAsync(int id)
        {
            var marker = await _markerRepository.GetByIdAsync(id);
            if (marker == null)
            {
                return false;
            }

            _markerRepository.Remove(marker);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<MapMarkerDto>> FindNearbyMarkersAsync(double latitude, double longitude, double radiusInMeters)
        {
            // Временное решение: вместо пространственного поиска используем простой поиск по координатам
            // Приблизительное расстояние 1 градуса широты в метрах (на экваторе)
            const double metersPerDegreeLat = 111111;
            
            // Приблизительное расстояние 1 градуса долготы на заданной широте в метрах
            // (зависит от широты, уменьшается от экватора к полюсам)
            double metersPerDegreeLon = metersPerDegreeLat * Math.Cos(latitude * Math.PI / 180);
            
            // Преобразуем радиус из метров в градусы для широты и долготы
            double latDelta = radiusInMeters / metersPerDegreeLat;
            double lonDelta = radiusInMeters / metersPerDegreeLon;
            
            // Получаем все маркеры
            var allMarkers = await _markerRepository.GetAllAsync();
            
            // Фильтруем маркеры, которые находятся в пределах указанного прямоугольника
            var nearbyMarkers = allMarkers.Where(m => 
                m.Latitude >= latitude - latDelta &&
                m.Latitude <= latitude + latDelta &&
                m.Longitude >= longitude - lonDelta &&
                m.Longitude <= longitude + lonDelta);
            
            // Для более точного поиска отфильтровываем результаты,
            // вычислив точное расстояние по формуле гаверсинусов
            var result = nearbyMarkers.Where(m => CalculateDistance(
                latitude, longitude, 
                m.Latitude, m.Longitude) <= radiusInMeters);
                
            return result.Select(MapMarkerToDto);
        }
        
        /// <summary>
        /// Вычисляет расстояние между двумя точками на земной поверхности (по формуле гаверсинусов)
        /// </summary>
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var d1 = lat1 * (Math.PI / 180.0);
            var num1 = lon1 * (Math.PI / 180.0);
            var d2 = lat2 * (Math.PI / 180.0);
            var num2 = lon2 * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                   Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            
            // Радиус Земли в метрах
            const double earthRadius = 6371000;
            return earthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        #endregion

        #region MapOptions

        public async Task<IEnumerable<MapOptionsDto>> GetAllOptionsAsync()
        {
            var options = await _optionsRepository.GetAllAsync();
            return options.Select(MapOptionsToDto);
        }

        public async Task<MapOptionsDto> GetOptionsByIdAsync(int id)
        {
            var options = await _optionsRepository.GetByIdAsync(id);
            if (options == null)
            {
                return null;
            }

            return MapOptionsToDto(options);
        }

        public async Task<MapOptionsDto> GetDefaultOptionsAsync()
        {
            var options = await _optionsRepository.GetDefaultAsync();
            if (options == null)
            {
                throw new KeyNotFoundException("Настройки карты по умолчанию не найдены");
            }
            return MapOptionsToDto(options);
        }

        public async Task<MapOptionsDto> CreateOptionsAsync(CreateMapOptionsDto dto)
        {
            var options = new MapOptions
            {
                Name = dto.Name,
                CenterLatitude = dto.CenterLatitude,
                CenterLongitude = dto.CenterLongitude,
                Zoom = dto.Zoom,
                MinZoom = dto.MinZoom ?? 1,
                MaxZoom = dto.MaxZoom ?? 18,
                SouthBound = dto.SouthBound,
                WestBound = dto.WestBound,
                NorthBound = dto.NorthBound,
                EastBound = dto.EastBound,
                IsDefault = dto.IsDefault
            };

            await _optionsRepository.AddAsync(options);
            await _unitOfWork.SaveChangesAsync();

            return MapOptionsToDto(options);
        }

        public async Task<MapOptionsDto> UpdateOptionsAsync(UpdateMapOptionsDto dto)
        {
            var options = await _optionsRepository.GetByIdAsync(dto.Id);
            if (options == null)
            {
                throw new KeyNotFoundException($"Настройки карты с Id {dto.Id} не найдены");
            }

            options.Name = dto.Name;
            options.CenterLatitude = dto.CenterLatitude;
            options.CenterLongitude = dto.CenterLongitude;
            options.Zoom = dto.Zoom;
            options.MinZoom = dto.MinZoom ?? 1;
            options.MaxZoom = dto.MaxZoom ?? 18;
            options.SouthBound = dto.SouthBound;
            options.WestBound = dto.WestBound;
            options.NorthBound = dto.NorthBound;
            options.EastBound = dto.EastBound;
            options.IsDefault = dto.IsDefault;

            _optionsRepository.Update(options);
            await _unitOfWork.SaveChangesAsync();

            return MapOptionsToDto(options);
        }

        public async Task<bool> DeleteOptionsAsync(int id)
        {
            var options = await _optionsRepository.GetByIdAsync(id);
            if (options == null)
            {
                return false;
            }

            if (options.IsDefault)
            {
                throw new InvalidOperationException("Нельзя удалить настройки карты по умолчанию");
            }

            _optionsRepository.Remove(options);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        #endregion

        #region MapLayers

        public async Task<IEnumerable<MapLayerDto>> GetAllLayersAsync()
        {
            var layers = await _layerRepository.GetAllAsync();
            return layers.Select(MapLayerToDto);
        }

        public async Task<MapLayerDto> GetLayerByIdAsync(int id)
        {
            var layer = await _layerRepository.GetByIdAsync(id);
            if (layer == null)
            {
                return null;
            }

            return MapLayerToDto(layer);
        }

        public async Task<MapLayerDto> GetDefaultLayerAsync()
        {
            var layer = await _layerRepository.GetDefaultActiveLayerAsync();
            if (layer == null)
            {
                throw new KeyNotFoundException("Слой карты по умолчанию не найден");
            }
            return MapLayerToDto(layer);
        }

        public async Task<MapLayerDto> CreateLayerAsync(CreateMapLayerDto dto)
        {
            var existingLayer = await _layerRepository.FindByNameAsync(dto.Name);
            if (existingLayer != null)
            {
                throw new InvalidOperationException($"Слой с именем '{dto.Name}' уже существует");
            }

            var layer = new MapLayer
            {
                Name = dto.Name,
                Description = dto.Description,
                BaseDirectory = $"/maps/{dto.Name.ToLower().Replace(" ", "-")}",
                MinZoom = 1,
                MaxZoom = 18,
                TileFormat = "png",
                IsActive = dto.IsDefault
            };

            await _layerRepository.AddAsync(layer);
            await _unitOfWork.SaveChangesAsync();

            return MapLayerToDto(layer);
        }

        public async Task<MapLayerDto> UpdateLayerAsync(UpdateMapLayerDto dto)
        {
            var layer = await _layerRepository.GetByIdAsync(dto.Id);
            if (layer == null)
            {
                throw new KeyNotFoundException($"Слой карты с Id {dto.Id} не найден");
            }

            if (dto.Name != layer.Name)
            {
                var existingLayer = await _layerRepository.FindByNameAsync(dto.Name);
                if (existingLayer != null && existingLayer.Id != dto.Id)
                {
                    throw new InvalidOperationException($"Слой с именем '{dto.Name}' уже существует");
                }
            }

            layer.Name = dto.Name;
            layer.Description = dto.Description;
            layer.IsActive = dto.IsDefault;

            _layerRepository.Update(layer);
            await _unitOfWork.SaveChangesAsync();

            return MapLayerToDto(layer);
        }

        public async Task<bool> DeleteLayerAsync(int id)
        {
            var layer = await _layerRepository.GetByIdAsync(id);
            if (layer == null)
            {
                return false;
            }

            if (layer.IsActive)
            {
                throw new InvalidOperationException("Нельзя удалить активный слой карты");
            }

            _layerRepository.Remove(layer);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        #endregion

        #region MapAreas

        // Примечание: Эти методы являются заглушками, поскольку в системе пока нет репозитория для MapArea
        // Необходимо реализовать соответствующий репозиторий и заменить эти методы фактической реализацией

        public async Task<IEnumerable<MapAreaDto>> GetAllAreasAsync()
        {
            // Временная реализация
            await Task.CompletedTask;
            return new List<MapAreaDto>();
        }

        public async Task<MapAreaDto> GetAreaByIdAsync(int id)
        {
            // Временная реализация
            await Task.CompletedTask;
            return null;
        }

        public async Task<IEnumerable<MapAreaDto>> GetAreasByTypeAsync(AreaType type)
        {
            // Временная реализация
            await Task.CompletedTask;
            return new List<MapAreaDto>();
        }

        public async Task<MapAreaDto> CreateAreaAsync(CreateMapAreaDto areaDto)
        {
            // Временная реализация
            await Task.CompletedTask;
            throw new NotImplementedException("Функциональность создания зон карты еще не реализована");
        }

        public async Task<MapAreaDto> UpdateAreaAsync(UpdateMapAreaDto areaDto)
        {
            // Временная реализация
            await Task.CompletedTask;
            throw new NotImplementedException("Функциональность обновления зон карты еще не реализована");
        }

        public async Task<bool> DeleteAreaAsync(int id)
        {
            // Временная реализация
            await Task.CompletedTask;
            throw new NotImplementedException("Функциональность удаления зон карты еще не реализована");
        }

        #endregion

        #region Private Methods

        private MapMarkerDto MapMarkerToDto(MapMarker marker)
        {
            return new MapMarkerDto
            {
                Id = marker.Id,
                Latitude = marker.Latitude,
                Longitude = marker.Longitude,
                Title = marker.Title,
                Type = marker.Type,
                Description = marker.Description,
                PopupContent = marker.PopupContent,
                SpecimenId = marker.SpecimenId,
                RegionId = marker.RegionId,
                CreatedAt = marker.CreatedAt,
                UpdatedAt = marker.UpdatedAt
            };
        }

        private MapOptionsDto MapOptionsToDto(MapOptions options)
        {
            return new MapOptionsDto
            {
                Id = options.Id,
                Name = options.Name,
                CenterLatitude = options.CenterLatitude,
                CenterLongitude = options.CenterLongitude,
                Zoom = options.Zoom,
                MinZoom = options.MinZoom,
                MaxZoom = options.MaxZoom,
                SouthBound = options.SouthBound,
                WestBound = options.WestBound,
                NorthBound = options.NorthBound,
                EastBound = options.EastBound,
                IsDefault = options.IsDefault,
                CreatedAt = options.CreatedAt,
                UpdatedAt = options.UpdatedAt
            };
        }

        private MapLayerDto MapLayerToDto(MapLayer layer)
        {
            return new MapLayerDto
            {
                Id = layer.Id,
                Name = layer.Name,
                Description = layer.Description,
                Url = $"/api/map/tiles/{layer.Id}/{"{z}/{x}/{y}.{format}"}",
                Attribution = "Botanical Garden",
                IsDefault = layer.IsActive,
                DisplayOrder = 0
            };
        }

        private Point CreatePoint(double latitude, double longitude)
        {
            return new Point(longitude, latitude) { SRID = 4326 };
        }

        #endregion
    }
} 