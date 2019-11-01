using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace Catalog.Infrastructure.Extensions
{
    public static class EventsExtensions
    {
        public static async Task<IServiceCollection> AddRabbitMq(this IServiceCollection services, 
            string endpointName, string connectionString, string environmentName)
        {
            if (environmentName.Equals("Testing")) return services;
            var maxRetryAttempts = 5;
            var pauseBetweenFailures = TimeSpan.FromSeconds(20);
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

            var endpointInstance =  await retryPolicy.ExecuteAsync(async ()=> await ServiceCollection(endpointName, connectionString));
            services.AddSingleton(endpointInstance);

            return services;
        }

        private static async Task<IEndpointInstance> ServiceCollection(string endpointName, string connectionString)
        {
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
            
            return await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
     
        }
    }
}