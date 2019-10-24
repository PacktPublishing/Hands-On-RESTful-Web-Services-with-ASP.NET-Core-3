using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Routing;

namespace Catalog.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static async Task<IServiceCollection> AddRabbitMq(this IServiceCollection services, 
            string endpointName, string connectionString, string environmentName)
        {

            if (environmentName.Equals("Testing")) return services;
            
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>()
                .UseConventionalRoutingTopology();

            transport.ConnectionString(connectionString);
            
            var routing = transport
                .Routing();
            
            routing.RouteToEndpoint(typeof(ItemSoldOutEvent), endpointName);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(
                type => type.Namespace != null && type.Namespace.Contains("Catalog.Infrastructure"));
                     
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ScanAssembliesInNestedDirectories = true;
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            services.AddSingleton(endpointInstance);

            return services;
        }
    }
}