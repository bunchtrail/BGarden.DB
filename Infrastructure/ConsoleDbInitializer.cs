using BGarden.Infrastructure.Data;
using BGarden.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BGarden.Infrastructure
{
    /// <summary>
    /// Консольное приложение для инициализации базы данных.
    /// Может использоваться как отдельный инструмент.
    /// </summary>
    public class ConsoleDbInitializer
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Запуск инициализации базы данных ботанического сада...");

            try
            {
                // Создаем контекст БД
                var factory = new DesignTimeDbContextFactory();
                var context = factory.CreateDbContext(args);

                Console.WriteLine("Контекст базы данных создан успешно");
                Console.WriteLine($"Строка подключения: {ConnectionString.PostgreSQL}");

                // Проверяем, можно ли подключиться к базе данных
                bool canConnect = await context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    Console.WriteLine("Невозможно подключиться к базе данных. Проверьте строку подключения и доступность сервера.");
                    return;
                }
                
                Console.WriteLine("Подключение к базе данных успешно");

                // Инициализируем базу данных с помощью обновленного метода
                await DbInitializationExtensions.InitializeDatabaseAsync(context);

                // Вывод информации о содержимом базы
                int familiesCount = await context.Families.CountAsync();
                int expositionsCount = await context.Expositions.CountAsync();
                int regionsCount = await context.Regions.CountAsync();
                int specimensCount = await context.Specimens.CountAsync();

                Console.WriteLine("\nИнформация о содержимом базы данных:");
                Console.WriteLine($"- Семейства: {familiesCount}");
                Console.WriteLine($"- Экспозиции: {expositionsCount}");
                Console.WriteLine($"- Регионы: {regionsCount}");
                Console.WriteLine($"- Образцы растений: {specimensCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при инициализации базы данных: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nПроцесс инициализации завершен. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
} 