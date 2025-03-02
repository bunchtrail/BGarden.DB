using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;

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

            // Регистрируем UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
} 