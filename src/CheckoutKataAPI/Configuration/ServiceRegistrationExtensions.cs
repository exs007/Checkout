using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CheckoutKataAPI.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace CheckoutKataAPI.Configuration
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(MemoryRepository<>));

            return services;
        }
    }
}
