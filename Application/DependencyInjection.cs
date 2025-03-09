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

namespace BGarden.Application
{
    /// <summary>
    /// Класс для настройки внедрения зависимостей в слое Application
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Метод расширения для добавления служб уровня приложения
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Регистрация сервисов
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISpecimenService, SpecimenService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<IExpositionService, ExpositionService>();
            services.AddScoped<IPhenologyService, PhenologyService>();
            services.AddScoped<IBiometryService, BiometryService>();
            services.AddScoped<IRegionService, RegionService>();

            // Регистрация вариантов использования
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<CreateSpecimenUseCase>();
            
            return services;
        }
    }
} 