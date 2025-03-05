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
    /// Класс для добавления образцов растений в базу данных
    /// </summary>
    public static class SpecimenSeeder
    {
        /// <summary>
        /// Добавляет образцы растений в базу данных
        /// </summary>
        public static async Task SeedSpecimensAsync(BotanicalContext context)
        {
            try
            {
                // Проверяем, есть ли образцы в базе
                if (await context.Specimens.AnyAsync())
                {
                    Console.WriteLine("Образцы растений уже существуют в базе данных");
                    return;
                }
                
                // Проверяем наличие необходимых данных
                Console.WriteLine("Проверка наличия необходимых данных в базе...");
                
                bool hasFamilies = await context.Families.AnyAsync();
                bool hasExpositions = await context.Expositions.AnyAsync();
                bool hasRegions = await context.Regions.AnyAsync();
                
                Console.WriteLine($"Наличие семейств: {hasFamilies}");
                Console.WriteLine($"Наличие экспозиций: {hasExpositions}");
                Console.WriteLine($"Наличие регионов: {hasRegions}");
                
                if (!hasFamilies || !hasExpositions || !hasRegions)
                {
                    Console.WriteLine("Не найдены необходимые данные (семейства, экспозиции или регионы)");
                    return;
                }
                
                // Получаем необходимые данные из базы
                var families = await context.Families.ToListAsync();
                var expositions = await context.Expositions.ToListAsync();
                var regions = await context.Regions.ToListAsync();
                
                Console.WriteLine("Загруженные данные:");
                Console.WriteLine($"Количество семейств: {families.Count}");
                Console.WriteLine($"Количество экспозиций: {expositions.Count}");
                Console.WriteLine($"Количество регионов: {regions.Count}");
                
                // Выводим список всех экспозиций для диагностики
                Console.WriteLine("Список всех экспозиций в базе данных:");
                foreach (var expo in expositions)
                {
                    Console.WriteLine($"ID: {expo.Id}, Название: '{expo.Name}'");
                }
                
                if (families.Count == 0 || expositions.Count == 0 || regions.Count == 0)
                {
                    Console.WriteLine("Не найдены необходимые данные (пустые списки)");
                    Console.WriteLine($"Семейства: {families.Count}, Экспозиции: {expositions.Count}, Регионы: {regions.Count}");
                    return;
                }
                
                // Находим нужные семейства (используем Contains для большей гибкости)
                var pinaceae = families.FirstOrDefault(f => f.Name.Contains("Сосновые", StringComparison.OrdinalIgnoreCase)); 
                var rosaceae = families.FirstOrDefault(f => f.Name.Contains("Розоцветные", StringComparison.OrdinalIgnoreCase));
                var asteraceae = families.FirstOrDefault(f => f.Name.Contains("Астровые", StringComparison.OrdinalIgnoreCase) || 
                                                              f.Name.Contains("Сложноцветные", StringComparison.OrdinalIgnoreCase));
                
                if (pinaceae == null)
                {
                    Console.WriteLine("Не найдено семейство Сосновые");
                    Console.WriteLine("Доступные семейства:");
                    foreach (var family in families)
                    {
                        Console.WriteLine($"- {family.Name}");
                    }
                    return;
                }
                
                if (rosaceae == null)
                {
                    Console.WriteLine("Не найдено семейство Розоцветные");
                    return;
                }
                
                if (asteraceae == null)
                {
                    Console.WriteLine("Не найдено семейство Астровые");
                    return;
                }
                
                // Находим нужные экспозиции (используем Contains для большей гибкости)
                var conifers = expositions.FirstOrDefault(e => e.Name.Contains("Хвойные", StringComparison.OrdinalIgnoreCase));
                var rockGarden = expositions.FirstOrDefault(e => e.Name.Contains("Альпийская", StringComparison.OrdinalIgnoreCase));
                var rosarium = expositions.FirstOrDefault(e => e.Name.Contains("Розарий", StringComparison.OrdinalIgnoreCase));
                
                if (conifers == null)
                {
                    Console.WriteLine("Не найдена экспозиция Хвойные");
                    // Если не найдена нужная экспозиция, берем первую попавшуюся для дендрологии
                    conifers = expositions.FirstOrDefault() ?? throw new Exception("Не найдено ни одной экспозиции");
                    Console.WriteLine($"Использую альтернативную экспозицию: {conifers.Name}");
                }
                
                if (rockGarden == null)
                {
                    Console.WriteLine("Не найдена экспозиция Альпийская горка");
                    // Если не найдена нужная экспозиция, берем первую попавшуюся
                    rockGarden = expositions.ElementAtOrDefault(1) ?? expositions.FirstOrDefault() ?? 
                                throw new Exception("Не найдено подходящей экспозиции для Альпийской горки");
                    Console.WriteLine($"Использую альтернативную экспозицию: {rockGarden.Name}");
                }
                
                if (rosarium == null)
                {
                    Console.WriteLine("Не найдена экспозиция Розарий");
                    // Если не найдена нужная экспозиция, берем первую попавшуюся
                    rosarium = expositions.ElementAtOrDefault(2) ?? expositions.FirstOrDefault() ?? 
                              throw new Exception("Не найдено подходящей экспозиции для Розария");
                    Console.WriteLine($"Использую альтернативную экспозицию: {rosarium.Name}");
                }
                
                // Находим нужные регионы
                var dendrologyRegion = regions.FirstOrDefault(r => r.SectorType == SectorType.Dendrology);
                var floraRegion = regions.FirstOrDefault(r => r.SectorType == SectorType.Flora);
                var floweringRegion = regions.FirstOrDefault(r => r.SectorType == SectorType.Flowering);
                
                if (dendrologyRegion == null)
                {
                    Console.WriteLine("Не найден регион для Дендрологии");
                    // Пробуем найти по имени
                    dendrologyRegion = regions.FirstOrDefault(r => r.Name.Contains("Дендр", StringComparison.OrdinalIgnoreCase)) ?? 
                                      regions.FirstOrDefault();
                }
                
                if (floraRegion == null)
                {
                    Console.WriteLine("Не найден регион для Флоры");
                    // Пробуем найти по имени
                    floraRegion = regions.FirstOrDefault(r => r.Name.Contains("Флора", StringComparison.OrdinalIgnoreCase)) ?? 
                                 regions.FirstOrDefault();
                }
                
                if (floweringRegion == null)
                {
                    Console.WriteLine("Не найден регион для Цветоводства");
                    // Пробуем найти по имени
                    floweringRegion = regions.FirstOrDefault(r => r.Name.Contains("Цвет", StringComparison.OrdinalIgnoreCase)) ?? 
                                     regions.FirstOrDefault();
                }
                
                Console.WriteLine("Найдены все необходимые данные для создания образцов");
                Console.WriteLine($"Семейства: {pinaceae.Name}, {rosaceae.Name}, {asteraceae.Name}");
                Console.WriteLine($"Экспозиции: {conifers.Name} (ID: {conifers.Id}), {rockGarden.Name} (ID: {rockGarden.Id}), {rosarium.Name} (ID: {rosarium.Id})");
                Console.WriteLine($"Регионы: {dendrologyRegion?.Name}, {floraRegion?.Name}, {floweringRegion?.Name}");
                
                // Создаем образцы растений
                var specimens = new List<Specimen>
                {
                    // Дендрология
                    new Specimen
                    {
                        InventoryNumber = "D-001",
                        SectorType = SectorType.Dendrology,
                        FamilyId = pinaceae.Id,
                        Genus = "Pinus",
                        Species = "Pinus sylvestris",
                        RussianName = "Сосна обыкновенная",
                        PlantingYear = 2020,
                        ExpositionId = conifers.Id,
                        RegionId = dendrologyRegion?.Id,
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
                        FamilyId = asteraceae.Id,
                        Genus = "Aster",
                        Species = "Aster alpinus",
                        RussianName = "Астра альпийская",
                        PlantingYear = 2021,
                        ExpositionId = rockGarden.Id,
                        RegionId = floraRegion?.Id,
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
                        FamilyId = rosaceae.Id,
                        Genus = "Rosa",
                        Species = "Rosa hybrida",
                        Cultivar = "Peace",
                        RussianName = "Роза 'Пис'",
                        PlantingYear = 2022,
                        ExpositionId = rosarium.Id,
                        RegionId = floweringRegion?.Id,
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

                // Проверяем наличие всех необходимых внешних ключей
                foreach (var specimen in specimens)
                {
                    Console.WriteLine($"Проверка данных для образца {specimen.InventoryNumber}:");
                    Console.WriteLine($"FamilyId: {specimen.FamilyId}, ExpositionId: {specimen.ExpositionId}, RegionId: {specimen.RegionId}");
                }
                
                // Устанавливаем координаты для образцов
                foreach (var specimen in specimens)
                {
                    var region = regions.FirstOrDefault(r => r.Id == specimen.RegionId);
                    if (region != null)
                    {
                        // Немного смещаем от центра региона
                        specimen.Latitude = region.Latitude + 0.001m * (new Random().Next(-5, 5));
                        specimen.Longitude = region.Longitude + 0.001m * (new Random().Next(-5, 5));
                        try
                        {
                            specimen.Location = new Point(Convert.ToDouble(specimen.Longitude), Convert.ToDouble(specimen.Latitude))
                            {
                                SRID = 4326 // WGS 84
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при создании Point для образца {specimen.InventoryNumber}: {ex.Message}");
                        }
                    }
                }
                
                Console.WriteLine("Добавление образцов растений...");
                try {
                    await context.Specimens.AddRangeAsync(specimens);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Добавлено образцов: {specimens.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сохранении образцов в базу данных: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                    }
                    throw; // Пробрасываем исключение дальше для отладки
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении образцов: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                    Console.WriteLine(ex.InnerException.StackTrace);
                }
                throw; // Пробрасываем исключение дальше для отладки
            }
        }
    }
} 