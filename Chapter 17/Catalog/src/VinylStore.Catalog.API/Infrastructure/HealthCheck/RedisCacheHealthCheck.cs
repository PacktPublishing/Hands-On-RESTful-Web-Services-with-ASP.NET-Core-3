using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VinylStore.Catalog.Domain;
using VinylStore.Catalog.Domain.Infrastructure.Settings;

namespace VinylStore.Catalog.API.Infrastructure.HealthCheck
{
    public class RedisCacheHealthCheck : IHealthCheck
    {
        private readonly CacheSettings _settings;

        public RedisCacheHealthCheck(IOptions<CacheSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect(_settings.ConnectionString);
                var db = redis.GetDatabase();

                var result = await db.PingAsync();
                if (result < TimeSpan.FromSeconds(5))
                {
                    return await Task.FromResult(
                        HealthCheckResult.Healthy());
                }

                return await Task.FromResult(
                    HealthCheckResult.Unhealthy());
            }
            catch (Exception e)
            {
                return await Task.FromResult(
                    HealthCheckResult.Unhealthy(e.Message));
            }
        }
    }
}
