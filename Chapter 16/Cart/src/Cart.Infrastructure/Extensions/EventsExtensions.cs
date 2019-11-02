using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Cart.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, string endpointName,
            string connectionString, string environmentName)
        {
            if (environmentName.Equals("Testing")) return services;
            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration
                .UseTransport<RabbitMQTransport>()
                .UseConventionalRoutingTopology()
                .ConnectionString(connectionString);

            endpointConfiguration.UseContainer<ServicesBuilder>(
                customizations => { customizations.ExistingServices(services); });

            endpointConfiguration.EnableInstallers();

            endpointConfiguration
                .Conventions()
                .DefiningEventsAs(
                    type => type.Namespace?.Contains("Cart.Events") ?? false);

            var endpointInstance = Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(endpointInstance);

            return services;
        }
    }
}