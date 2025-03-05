using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BGarden.Infrastructure
{
    /// <summary>
    /// Точка входа приложения, выбирающая какую инициализацию запустить
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Основная точка входа приложения
        /// </summary>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Получены следующие аргументы командной строки:");
            if (args.Length == 0)
            {
                Console.WriteLine("Аргументы отсутствуют");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"Аргумент {i}: '{args[i]}'");
                }
            }
            
            // Выбираем, какую программу запустить в зависимости от аргументов
            if (args.Contains("-initialize-db") || args.Contains("--initialize-db"))
            {
                Console.WriteLine("Запускаем ConsoleDbInitializer для инициализации базы данных...");
                await ConsoleDbInitializer.Main(args);
            }
            else if (args.Contains("-add-specimens") || args.Contains("--add-specimens"))
            {
                Console.WriteLine("Запускаем SpecimenInitializer для добавления образцов растений...");
                await SpecimenInitializer.Main(args);
            }
            else
            {
                Console.WriteLine("Аргументы не распознаны, запускаем ConsoleDbInitializer по умолчанию...");
                // По умолчанию запускаем ConsoleDbInitializer
                await ConsoleDbInitializer.Main(args);
            }
        }

        /// <summary>
        /// Запускает вспомогательные функции для миграций
        /// </summary>
        public static void RunMigrationHelpers(string[] args)
        {
            // Создание контекста БД для миграций
            var factory = new DesignTimeDbContextFactory();
            var context = factory.CreateDbContext(args);

            Console.WriteLine("Контекст базы данных создан успешно");
            Console.WriteLine($"Строка подключения: {ConnectionString.PostgreSQL}");
            Console.WriteLine("Программа готова к управлению миграциями");
        }
    }
} 