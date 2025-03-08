using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;
using BGarden.Infrastructure.Services;
using BGarden.DB.Domain.Interfaces;
using BGarden.DB.Infrastructure.Repositories;

namespace BGarden.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрируем DbContext
            services.AddDbContext<BotanicalContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(BotanicalContext).Assembly.FullName)));

            // Регистрируем репозитории
            services.AddScoped<ISpecimenRepository, SpecimenRepository>();
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IExpositionRepository, ExpositionRepository>();
            services.AddScoped<IBiometryRepository, BiometryRepository>();
            services.AddScoped<IPhenologyRepository, PhenologyRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            // Регистрируем репозитории для модуля карты
            services.AddScoped<IMapMarkerRepository, MapMarkerRepository>();
            services.AddScoped<IMapOptionsRepository, MapOptionsRepository>();

            // Регистрируем UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Регистрируем сервисы
            services.AddScoped<IAuthService>(provider => 
                new AuthService(
                    provider.GetRequiredService<BotanicalContext>(),
                    configuration
                )
            );

            return services;
        }
    }
} 