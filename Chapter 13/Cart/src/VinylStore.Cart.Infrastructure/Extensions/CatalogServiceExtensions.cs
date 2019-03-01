using System;
using Microsoft.Extensions.DependencyInjection;
using VinylStore.Cart.Domain.Services;
using VinylStore.Cart.Infrastructure.Extensions.Policies;
using VinylStore.Cart.Infrastructure.Services;
using VinylStore.Catalog.API.Client;

namespace VinylStore.Cart.Infrastructure.Extensions
{
    public static class CatalogServiceExtensions
    {
        public static IServiceCollection AddCatalogService(this IServiceCollection services, Uri uri)
        {
            services.AddScoped<ICatalogService, CatalogService>();

            services.AddHttpClient<ICatalogClient, CatalogClient>(client => { client.BaseAddress = uri; })
                .SetHandlerLifetime(TimeSpan.FromMinutes(2)) //Set lifetime to five minutes
                .AddPolicyHandler(CatalogServicePolicies.RetryPolicy())
                .AddPolicyHandler(CatalogServicePolicies.CircuitBreakerPolicy());

            return services;
        }
    }
}
