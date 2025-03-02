using BGarden.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// Этот файл необходим для поддержки миграций EF Core
// Он не используется при обычной работе приложения

// Создание контекста БД для миграций
var factory = new DesignTimeDbContextFactory();
var context = factory.CreateDbContext(args);

Console.WriteLine("Контекст базы данных создан успешно");
Console.WriteLine($"Строка подключения: {ConnectionString.PostgreSQL}");
Console.WriteLine("Программа готова к управлению миграциями"); 