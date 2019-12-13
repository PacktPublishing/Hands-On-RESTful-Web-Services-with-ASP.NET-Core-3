using System;
using Cart.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Cart.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static IServiceCollection ConfigureEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventBusSettings>(configuration.GetSection("EventBus"));
            return services;
        }
    }
}