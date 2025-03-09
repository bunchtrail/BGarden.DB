using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Application.Services;
using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using BGarden.Domain.Interfaces;
using Moq;
using NetTopologySuite.Geometries;
using Xunit;

namespace BGarden.Tests
{
    public class SpecimenServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ISpecimenRepository> _mockSpecimenRepository;
        private readonly SpecimenService _specimenService;

        public SpecimenServiceTests()
        {
            _mockSpecimenRepository = new Mock<ISpecimenRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(uow => uow.Specimens).Returns(_mockSpecimenRepository.Object);
            _specimenService = new SpecimenService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task AddSpecimenToMap_WithValidCoordinates_ShouldCreateSpecimen()
        {
            // Arrange
            var specimenDto = new SpecimenDto
            {
                InventoryNumber = "TEST-001",
                SectorType = SectorType.Dendrology,
                Latitude = 55.123m,
                Longitude = 37.456m,
                FamilyId = 1,
                RussianName = "Тестовое растение",
                LatinName = "Testus plantus"
            };

            var createdSpecimen = new Specimen
            {
                Id = 1,
                InventoryNumber = specimenDto.InventoryNumber,
                SectorType = specimenDto.SectorType,
                Latitude = specimenDto.Latitude,
                Longitude = specimenDto.Longitude,
                FamilyId = specimenDto.FamilyId,
                RussianName = specimenDto.RussianName,
                LatinName = specimenDto.LatinName,
                Location = new Point((double)specimenDto.Longitude, (double)specimenDto.Latitude) { SRID = 4326 }
            };

            _mockSpecimenRepository.Setup(repo => repo.AddAsync(It.IsAny<Specimen>()))
                .Callback<Specimen>(s => 
                {
                    s.Id = 1;
                    s.Location = new Point((double)s.Longitude, (double)s.Latitude) { SRID = 4326 };
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _specimenService.AddSpecimenToMapAsync(specimenDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(specimenDto.InventoryNumber, result.InventoryNumber);
            Assert.Equal(specimenDto.Latitude, result.Latitude);
            Assert.Equal(specimenDto.Longitude, result.Longitude);
            _mockSpecimenRepository.Verify(repo => repo.AddAsync(It.IsAny<Specimen>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSpecimenLocation_WithValidId_ShouldUpdateLocation()
        {
            // Arrange
            int specimenId = 1;
            decimal newLatitude = 56.789m;
            decimal newLongitude = 38.123m;

            var existingSpecimen = new Specimen
            {
                Id = specimenId,
                InventoryNumber = "TEST-001",
                SectorType = SectorType.Dendrology,
                Latitude = 55.123m,
                Longitude = 37.456m,
                FamilyId = 1,
                RussianName = "Тестовое растение",
                LatinName = "Testus plantus",
                Location = new Point(37.456, 55.123) { SRID = 4326 }
            };

            _mockSpecimenRepository.Setup(repo => repo.GetByIdAsync(specimenId))
                .ReturnsAsync(existingSpecimen);

            // Act
            var result = await _specimenService.UpdateSpecimenLocationAsync(specimenId, newLatitude, newLongitude);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(specimenId, result.Id);
            Assert.Equal(newLatitude, result.Latitude);
            Assert.Equal(newLongitude, result.Longitude);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSpecimensInBoundingBox_ShouldReturnSpecimensInArea()
        {
            // Arrange
            decimal minLat = 55.0m;
            decimal minLng = 37.0m;
            decimal maxLat = 56.0m;
            decimal maxLng = 38.0m;

            var specimens = new List<Specimen>
            {
                new Specimen
                {
                    Id = 1,
                    InventoryNumber = "TEST-001",
                    SectorType = SectorType.Dendrology,
                    Latitude = 55.5m,
                    Longitude = 37.5m,
                    FamilyId = 1,
                    RussianName = "Тестовое растение 1",
                    LatinName = "Testus plantus 1",
                    Location = new Point(37.5, 55.5) { SRID = 4326 }
                },
                new Specimen
                {
                    Id = 2,
                    InventoryNumber = "TEST-002",
                    SectorType = SectorType.Flowering,
                    Latitude = 55.7m,
                    Longitude = 37.8m,
                    FamilyId = 2,
                    RussianName = "Тестовое растение 2",
                    LatinName = "Testus plantus 2",
                    Location = new Point(37.8, 55.7) { SRID = 4326 }
                }
            };

            _mockSpecimenRepository.Setup(repo => repo.GetSpecimensInBoundingBoxAsync(It.IsAny<Envelope>()))
                .ReturnsAsync(specimens);

            // Act
            var result = await _specimenService.GetSpecimensInBoundingBoxAsync(minLat, minLng, maxLat, maxLng);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Id == 1);
            Assert.Contains(result, s => s.Id == 2);
        }

        [Fact]
        public async Task DeleteSpecimen_WithValidId_ShouldRemoveSpecimen()
        {
            // Arrange
            int specimenId = 1;

            var existingSpecimen = new Specimen
            {
                Id = specimenId,
                InventoryNumber = "TEST-001",
                SectorType = SectorType.Dendrology,
                Latitude = 55.123m,
                Longitude = 37.456m,
                FamilyId = 1,
                RussianName = "Тестовое растение",
                LatinName = "Testus plantus",
                Location = new Point(37.456, 55.123) { SRID = 4326 }
            };

            _mockSpecimenRepository.Setup(repo => repo.GetByIdAsync(specimenId))
                .ReturnsAsync(existingSpecimen);

            // Act
            var result = await _specimenService.DeleteSpecimenAsync(specimenId);

            // Assert
            Assert.True(result);
            _mockSpecimenRepository.Verify(repo => repo.Remove(existingSpecimen), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
} 