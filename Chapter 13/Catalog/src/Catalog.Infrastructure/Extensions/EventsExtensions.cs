using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Catalog.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static async Task<IServiceCollection> AddRabbitMq(this IServiceCollection services, string endpointName, string connectionString)
        {

            var endpointConfiguration = new EndpointConfiguration(endpointName);
            
            endpointConfiguration.SendFailedMessagesTo("error");
            
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>()
                .UseConventionalRoutingTopology();

            transport.ConnectionString(connectionString);
            
            var routing = transport
                .Routing();
            
            routing.RouteToEndpoint(typeof(ItemSoldOutEvent), endpointName);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(
                type => type.Namespace != null && type.Namespace.Contains("Cart.Events"));
                     
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ScanAssembliesInNestedDirectories = true;
            
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            services.AddSingleton(endpointInstance);

            return services;
        }
    }
}