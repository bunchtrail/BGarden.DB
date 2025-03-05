using BGarden.Domain.Entities;
using BGarden.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGarden.Infrastructure.Data
{
    /// <summary>
    /// Класс для инициализации базы данных начальными данными
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Инициализирует базу данных начальными значениями, если они отсутствуют
        /// </summary>
        public static async Task SeedAsync(BotanicalContext context)
        {
            // Убедимся, что база данных создана
            await context.Database.EnsureCreatedAsync();
            
            // Проверим, есть ли уже данные в базе
            if (await context.Families.AnyAsync() || await context.Expositions.AnyAsync() || 
                await context.Regions.AnyAsync() || await context.Specimens.AnyAsync())
            {
                // База уже инициализирована
                return;
            }

            // Создаем типовые семейства
            var families = new List<Family>
            {
                new Family { Name = "Сосновые (Pinaceae)", Description = "Семейство хвойных растений" },
                new Family { Name = "Розоцветные (Rosaceae)", Description = "Семейство цветковых растений" },
                new Family { Name = "Вересковые (Ericaceae)", Description = "Семейство цветковых растений" },
                new Family { Name = "Кленовые (Aceraceae)", Description = "Семейство листопадных деревьев" },
                new Family { Name = "Астровые (Asteraceae)", Description = "Семейство цветковых растений" },
                new Family { Name = "Лилейные (Liliaceae)", Description = "Семейство однодольных растений" },
                new Family { Name = "Орхидные (Orchidaceae)", Description = "Семейство однодольных растений" },
                new Family { Name = "Бобовые (Fabaceae)", Description = "Семейство цветковых растений" },
                new Family { Name = "Злаковые (Poaceae)", Description = "Семейство однодольных растений" }
            };
            
            await context.Families.AddRangeAsync(families);
            await context.SaveChangesAsync();

            // Создаем экспозиции для каждого отдела
            var expositions = new List<Exposition>
            {
                // Дендрология
                new Exposition { Name = "Хвойные", Description = "Коллекция хвойных растений" },
                new Exposition { Name = "Лиственные", Description = "Коллекция лиственных деревьев и кустарников" },
                new Exposition { Name = "Вересковый сад", Description = "Коллекция растений семейства Вересковые" },
                
                // Флора
                new Exposition { Name = "Альпийская горка", Description = "Коллекция горных растений" },
                new Exposition { Name = "Степные растения", Description = "Коллекция растений степной зоны" },
                new Exposition { Name = "Лесные растения", Description = "Коллекция лесных растений" },
                
                // Цветоводство
                new Exposition { Name = "Розарий", Description = "Коллекция роз разных сортов" },
                new Exposition { Name = "Оранжерея тропических растений", Description = "Коллекция тропических растений" },
                new Exposition { Name = "Коллекция однолетников", Description = "Сезонная коллекция однолетних растений" }
            };
            
            await context.Expositions.AddRangeAsync(expositions);
            await context.SaveChangesAsync();

            // Создаем регионы для каждого отдела
            var regions = new List<Region>
            {
                // Дендрология
                new Region 
                { 
                    Name = "Дендрарий", 
                    Description = "Основная территория дендрологического отдела",
                    Latitude = 55.123m,
                    Longitude = 37.456m,
                    SectorType = SectorType.Dendrology
                },
                
                // Флора
                new Region 
                { 
                    Name = "Участок флоры", 
                    Description = "Основная территория отдела флоры",
                    Latitude = 55.124m,
                    Longitude = 37.457m,
                    SectorType = SectorType.Flora
                },
                
                // Цветоводство
                new Region 
                { 
                    Name = "Участок цветоводства", 
                    Description = "Основная территория отдела цветоводства",
                    Latitude = 55.125m,
                    Longitude = 37.458m,
                    SectorType = SectorType.Flowering
                }
            };
            
            // Устанавливаем точки местоположения
            foreach (var region in regions)
            {
                region.Location = new Point(Convert.ToDouble(region.Longitude), Convert.ToDouble(region.Latitude))
                {
                    SRID = 4326 // WGS 84
                };
            }
            
            await context.Regions.AddRangeAsync(regions);
            await context.SaveChangesAsync();

            // Создаем образцы растений для каждого отдела
            var specimens = new List<Specimen>
            {
                // Дендрология
                new Specimen
                {
                    InventoryNumber = "D-001",
                    SectorType = SectorType.Dendrology,
                    FamilyId = families[0].Id, // Сосновые
                    Genus = "Pinus",
                    Species = "Pinus sylvestris",
                    RussianName = "Сосна обыкновенная",
                    PlantingYear = 2020,
                    ExpositionId = expositions[0].Id, // Хвойные
                    RegionId = regions[0].Id, // Дендрарий
                    NaturalRange = "Европа, Сибирь",
                    EcologyAndBiology = "Светолюбивое растение, предпочитает песчаные почвы",
                    EconomicUse = "Древесина, смола, эфирные масла",
                    HasHerbarium = true,
                    ConservationStatus = "Не охраняется",
                    CreatedAt = DateTime.Now
                },
                
                // Флора
                new Specimen
                {
                    InventoryNumber = "F-001",
                    SectorType = SectorType.Flora,
                    FamilyId = families[4].Id, // Астровые
                    Genus = "Aster",
                    Species = "Aster alpinus",
                    RussianName = "Астра альпийская",
                    PlantingYear = 2021,
                    ExpositionId = expositions[3].Id, // Альпийская горка
                    RegionId = regions[1].Id, // Участок флоры
                    NaturalRange = "Горные системы Евразии",
                    EcologyAndBiology = "Многолетнее травянистое растение, предпочитает хорошо дренированные почвы",
                    EconomicUse = "Декоративное",
                    HasHerbarium = true,
                    ConservationStatus = "Не охраняется",
                    CreatedAt = DateTime.Now
                },
                
                // Цветоводство
                new Specimen
                {
                    InventoryNumber = "C-001",
                    SectorType = SectorType.Flowering,
                    FamilyId = families[1].Id, // Розоцветные
                    Genus = "Rosa",
                    Species = "Rosa hybrida",
                    Cultivar = "Peace",
                    RussianName = "Роза 'Пис'",
                    PlantingYear = 2022,
                    ExpositionId = expositions[6].Id, // Розарий
                    RegionId = regions[2].Id, // Участок цветоводства
                    OriginalBreeder = "Francis Meilland",
                    OriginalYear = 1945,
                    Country = "Франция",
                    EcologyAndBiology = "Кустарник высотой до 1,2 м, требует солнечного местоположения",
                    EconomicUse = "Декоративное, для срезки",
                    HasHerbarium = false,
                    ConservationStatus = "Не относится",
                    CreatedAt = DateTime.Now
                }
            };
            
            // Устанавливаем координаты для образцов
            foreach (var specimen in specimens)
            {
                var region = regions.FirstOrDefault(r => r.Id == specimen.RegionId);
                if (region != null)
                {
                    // Немного смещаем от центра региона
                    specimen.Latitude = region.Latitude + 0.001m * (new Random().Next(-5, 5));
                    specimen.Longitude = region.Longitude + 0.001m * (new Random().Next(-5, 5));
                    specimen.Location = new Point(Convert.ToDouble(specimen.Longitude), Convert.ToDouble(specimen.Latitude))
                    {
                        SRID = 4326 // WGS 84
                    };
                }
            }
            
            try
            {
                Console.WriteLine("Добавление образцов растений...");
                await context.Specimens.AddRangeAsync(specimens);
                await context.SaveChangesAsync();
                Console.WriteLine($"Добавлено образцов: {specimens.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении образцов: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
} 