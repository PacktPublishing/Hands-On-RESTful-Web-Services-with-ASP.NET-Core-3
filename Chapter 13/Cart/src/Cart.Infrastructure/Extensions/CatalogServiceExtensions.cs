using System;
using Cart.Domain.Services;
using Cart.Infrastructure.Extensions.Policies;
using Cart.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Catalog.API.Client;

namespace Cart.Infrastructure.Extensions
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