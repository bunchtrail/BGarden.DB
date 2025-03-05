using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BGarden.Infrastructure.Extensions
{
    /// <summary>
    /// Утилитарный класс для инициализации базы данных начальными данными
    /// </summary>
    public static class DbInitializationExtensions
    {
        /// <summary>
        /// Выполняет инициализацию базы данных начальными значениями
        /// </summary>
        /// <param name="context">Экземпляр контекста базы данных</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public static async Task InitializeDatabaseAsync(BotanicalContext context)
        {
            try
            {
                Console.WriteLine("Инициализация базы данных...");
                
                // Применение миграций (при необходимости)
                await context.Database.MigrateAsync();
                
                // Заполнение начальными данными
                await DbInitializer.SeedAsync(context);
                
                Console.WriteLine("Инициализация базы данных завершена успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
                // В реальном приложении здесь лучше использовать логирование
            }
        }
        
        /// <summary>
        /// Синхронная версия метода инициализации базы данных
        /// </summary>
        public static void InitializeDatabase(BotanicalContext context)
        {
            InitializeDatabaseAsync(context).GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Инициализирует базу данных, используя провайдер сервисов
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<BotanicalContext>();
            
            await InitializeDatabaseAsync(context);
        }
        
        /// <summary>
        /// Синхронная версия метода инициализации базы данных с провайдером сервисов
        /// </summary>
        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            InitializeDatabaseAsync(serviceProvider).GetAwaiter().GetResult();
        }
    }
} 