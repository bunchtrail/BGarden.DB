using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;
using BGarden.Infrastructure.Services;

using Microsoft.Extensions.Logging;

namespace BGarden.Infrastructure
{
    /// <summary>
    /// Класс для настройки внедрения зависимостей в слое инфраструктуры
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Метод расширения для добавления служб инфраструктуры
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрируем контекст базы данных с повышенным уровнем логирования
            services.AddDbContext<BotanicalContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? ConnectionString.PostgreSQL,
                    x => x.UseNetTopologySuite())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            // Добавляем сервис кэширования
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();

            // Регистрируем базовые репозитории
            services.AddScoped<SpecimenRepository>();
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IExpositionRepository, ExpositionRepository>();
            services.AddScoped<IBiometryRepository, BiometryRepository>();
            services.AddScoped<IPhenologyRepository, PhenologyRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            // Регистрируем декораторы с кэшированием
            services.AddScoped<ISpecimenRepository>(provider => new CachedSpecimenRepository(
                provider.GetRequiredService<SpecimenRepository>(),
                provider.GetRequiredService<ICacheService>(),
                provider.GetRequiredService<ILogger<CachedSpecimenRepository>>()));

            // Регистрируем UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Регистрируем сервисы
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
} 