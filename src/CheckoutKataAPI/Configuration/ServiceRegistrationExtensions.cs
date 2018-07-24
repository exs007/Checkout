using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CheckoutKataAPI.Configuration
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {   
            //setup fluent validation
            services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
            var currentAssembly = typeof(Startup).GetTypeInfo().Assembly;
            AssemblyScanner.FindValidatorsInAssemblies(new Assembly[]{currentAssembly}).ForEach(pair => {
                services.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
            });

            services.AddSingleton(typeof(IRepository<>), typeof(MemoryRepository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPromotionService, PromotionService>();

            return services;
        }
    }
}
