using Catalog.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Catalog.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration, string environmentName)
        {
            if (environmentName.Equals("Testing")) return services;

            var config = new EventBusSettings();
            configuration.Bind("EventBus", config);
            services.AddSingleton(config);

            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.User,
                Password = config.Password
            };

            services.AddSingleton(factory);

            return services;
        }
    }
}