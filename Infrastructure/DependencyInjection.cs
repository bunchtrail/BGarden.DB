using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BGarden.Domain.Interfaces;
using BGarden.Infrastructure.Data;
using BGarden.Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
using Application.UseCases;

namespace BGarden.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрируем DbContext
            services.AddDbContext<BotanicalContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(BotanicalContext).Assembly.FullName)));

            // Регистрируем репозитории
            services.AddScoped<ISpecimenRepository, SpecimenRepository>();
            // Здесь можно добавить другие репозитории по мере необходимости

            // Регистрируем UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Регистрируем сервисы Application:
            services.AddScoped<ISpecimenService, SpecimenService>();
            // services.AddScoped<IFamilyService, FamilyService>();
            // services.AddScoped<IExpositionService, ExpositionService>();
            
            // Регистрируем UseCases (если используются)
            services.AddScoped<CreateSpecimenUseCase>();
            // services.AddScoped<UpdateSpecimenUseCase>();
            // services.AddScoped<DeleteSpecimenUseCase>();
            
            return services;
        }
    }
} 