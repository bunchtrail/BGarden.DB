using System;
using System.Collections.Generic;
using System.Linq;
using Application.DTO;
using BGarden.Domain.Entities;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Application.Mappers
{
    public static class SpecimenMapper
    {
        private static readonly WKTReader _wktReader = new WKTReader();
        private static readonly WKTWriter _wktWriter = new WKTWriter();

        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static SpecimenDto ToDto(this Specimen entity)
        {
            var dto = new SpecimenDto
            {
                Id = entity.Id,
                InventoryNumber = entity.InventoryNumber,
                SectorType = entity.SectorType,
                LocationType = entity.LocationType,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                LocationWkt = entity.Location?.AsText(),
                MapId = entity.MapId,
                MapX = entity.MapX,
                MapY = entity.MapY,
                RegionId = entity.RegionId,
                RegionName = entity.Region?.Name,
                FamilyId = entity.FamilyId,
                FamilyName = entity.Family?.Name,
                RussianName = entity.RussianName,
                LatinName = entity.LatinName,
                Genus = entity.Genus,
                Species = entity.Species,
                Cultivar = entity.Cultivar,
                Form = entity.Form,
                Synonyms = entity.Synonyms,
                DeterminedBy = entity.DeterminedBy,
                PlantingYear = entity.PlantingYear,
                SampleOrigin = entity.SampleOrigin,
                NaturalRange = entity.NaturalRange,
                EcologyAndBiology = entity.EcologyAndBiology,
                EconomicUse = entity.EconomicUse,
                ConservationStatus = entity.ConservationStatus,
                ExpositionId = entity.ExpositionId,
                ExpositionName = entity.Exposition?.Name,
                HasHerbarium = entity.HasHerbarium,
                DuplicatesInfo = entity.DuplicatesInfo,
                OriginalBreeder = entity.OriginalBreeder,
                OriginalYear = entity.OriginalYear,
                Country = entity.Country,
                Illustration = entity.Illustration,
                Notes = entity.Notes,
                FilledBy = entity.FilledBy
            };

            // Если у нас есть пространственные данные, преобразуем их в WKT
            if (entity.Location != null)
            {
                dto.LocationWkt = _wktWriter.Write(entity.Location);
            }
            
            return dto;
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Specimen).
        /// </summary>
        public static Specimen ToEntity(this SpecimenDto dto)
        {
            var entity = new Specimen
            {
                Id = dto.Id,
                InventoryNumber = dto.InventoryNumber,
                SectorType = dto.SectorType,
                LocationType = dto.LocationType,
                FamilyId = dto.FamilyId,
                RussianName = dto.RussianName,
                LatinName = dto.LatinName,
                Genus = dto.Genus,
                Species = dto.Species,
                Cultivar = dto.Cultivar,
                Form = dto.Form,
                Synonyms = dto.Synonyms,
                DeterminedBy = dto.DeterminedBy,
                PlantingYear = dto.PlantingYear,
                SampleOrigin = dto.SampleOrigin,
                NaturalRange = dto.NaturalRange,
                EcologyAndBiology = dto.EcologyAndBiology,
                EconomicUse = dto.EconomicUse,
                ConservationStatus = dto.ConservationStatus,
                ExpositionId = dto.ExpositionId,
                HasHerbarium = dto.HasHerbarium,
                DuplicatesInfo = dto.DuplicatesInfo,
                OriginalBreeder = dto.OriginalBreeder,
                OriginalYear = dto.OriginalYear,
                Country = dto.Country,
                Illustration = dto.Illustration,
                Notes = dto.Notes,
                FilledBy = dto.FilledBy,
                CreatedAt = DateTime.UtcNow
            };

            // Устанавливаем координаты в зависимости от типа
            switch (dto.LocationType)
            {
                case BGarden.Domain.Enums.LocationType.Geographic:
                    if (dto.Latitude.HasValue && dto.Longitude.HasValue)
                    {
                        entity.SetGeographicCoordinates(dto.Latitude.Value, dto.Longitude.Value);
                    }
                    break;
                case BGarden.Domain.Enums.LocationType.SchematicMap:
                    if (dto.MapId.HasValue && dto.MapX.HasValue && dto.MapY.HasValue)
                    {
                        entity.SetSchematicCoordinates(dto.MapId.Value, dto.MapX.Value, dto.MapY.Value);
                    }
                    break;
                default:
                    entity.ClearCoordinates();
                    break;
            }

            return entity;
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this SpecimenDto dto, Specimen entity)
        {
            entity.InventoryNumber = dto.InventoryNumber;
            entity.SectorType = dto.SectorType;
            
            // Обновляем координаты в зависимости от типа
            switch (dto.LocationType)
            {
                case BGarden.Domain.Enums.LocationType.Geographic:
                    if (dto.Latitude.HasValue && dto.Longitude.HasValue)
                    {
                        entity.SetGeographicCoordinates(dto.Latitude.Value, dto.Longitude.Value);
                    }
                    break;
                case BGarden.Domain.Enums.LocationType.SchematicMap:
                    if (dto.MapId.HasValue && dto.MapX.HasValue && dto.MapY.HasValue)
                    {
                        entity.SetSchematicCoordinates(dto.MapId.Value, dto.MapX.Value, dto.MapY.Value);
                    }
                    break;
                default:
                    entity.ClearCoordinates();
                    break;
            }
            
            entity.RegionId = dto.RegionId;
            entity.FamilyId = dto.FamilyId;
            entity.RussianName = dto.RussianName;
            entity.LatinName = dto.LatinName;
            entity.Genus = dto.Genus;
            entity.Species = dto.Species;
            entity.Cultivar = dto.Cultivar;
            entity.Form = dto.Form;
            entity.Synonyms = dto.Synonyms;
            entity.DeterminedBy = dto.DeterminedBy;
            entity.PlantingYear = dto.PlantingYear;
            entity.SampleOrigin = dto.SampleOrigin;
            entity.NaturalRange = dto.NaturalRange;
            entity.EcologyAndBiology = dto.EcologyAndBiology;
            entity.EconomicUse = dto.EconomicUse;
            entity.ConservationStatus = dto.ConservationStatus;
            entity.ExpositionId = dto.ExpositionId;
            entity.HasHerbarium = dto.HasHerbarium;
            entity.DuplicatesInfo = dto.DuplicatesInfo;
            entity.OriginalBreeder = dto.OriginalBreeder;
            entity.OriginalYear = dto.OriginalYear;
            entity.Country = dto.Country;
            entity.Illustration = dto.Illustration;
            entity.Notes = dto.Notes;
            entity.FilledBy = dto.FilledBy;
            entity.LastUpdatedAt = DateTime.UtcNow;
        }
    }
} 