using Application.Interfaces;
using Application.Services;
using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BGarden.Application.Interfaces;
using BGarden.Application.Services;
using BGarden.Application.UseCases;
using BGarden.Application.UseCases.Interfaces;
using BGarden.Domain.Interfaces;
using System;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Регистрируем сервисы
            services.AddScoped<ISpecimenService, SpecimenService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IExpositionService, ExpositionService>();
            services.AddScoped<IBiometryService, BiometryService>();
            services.AddScoped<IPhenologyService, PhenologyService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IUserService, UserService>();
            
            // Регистрируем UseCase'ы
            services.AddScoped<CreateSpecimenUseCase>();
            // Другие use cases по мере необходимости
            
            // Регистрируем сервисы аутентификации
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            
            // Примечание: IJwtService теперь регистрируется в API слое
            
            return services;
        }
    }
} 