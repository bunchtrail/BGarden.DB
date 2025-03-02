using Application.Interfaces;
using Application.Services;
using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Регистрируем сервисы
            services.AddScoped<ISpecimenService, SpecimenService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IExpositionService, ExpositionService>();
            services.AddScoped<IBiometryService, BiometryService>();
            services.AddScoped<IPhenologyService, PhenologyService>();
            
            // Регистрируем UseCase'ы
            services.AddScoped<CreateSpecimenUseCase>();
            // Другие use cases по мере необходимости
            
            return services;
        }
    }
} 