using Application.DTO;
using BGarden.Domain.Entities;
using NetTopologySuite.Geometries;

namespace Application.Mappers
{
    public static class SpecimenMapper
    {
        /// <summary>
        /// Преобразовать доменную сущность в DTO.
        /// </summary>
        public static SpecimenDto ToDto(this Specimen entity)
        {
            return new SpecimenDto
            {
                Id = entity.Id,
                InventoryNumber = entity.InventoryNumber,
                SectorType = entity.SectorType,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
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
        }

        /// <summary>
        /// Преобразовать DTO в доменную сущность (новый объект Specimen).
        /// </summary>
        public static Specimen ToEntity(this SpecimenDto dto)
        {
            var specimen = new Specimen
            {
                // Id = не задаём вручную, обычно автогенерируется
                InventoryNumber = dto.InventoryNumber,
                SectorType = dto.SectorType,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                RegionId = dto.RegionId,
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
                FilledBy = dto.FilledBy
            };

            // Создаем геометрическую точку, если заданы координаты
            if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                specimen.Location = new Point((double)dto.Longitude.Value, (double)dto.Latitude.Value) 
                { 
                    SRID = 4326 // WGS84 - стандартная система координат для GPS
                };
            }

            return specimen;
        }

        /// <summary>
        /// Обновить существующую сущность (merge) на основе данных DTO.
        /// </summary>
        public static void UpdateEntity(this SpecimenDto dto, Specimen entity)
        {
            entity.InventoryNumber = dto.InventoryNumber;
            entity.SectorType = dto.SectorType;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
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

            // Обновляем геометрическую точку
            if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                entity.Location = new Point((double)dto.Longitude.Value, (double)dto.Latitude.Value) 
                { 
                    SRID = 4326 // WGS84
                };
            }
            else
            {
                entity.Location = null;
            }
        }
    }
} 