using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace VinylStore.Catalog.API.Infrastructure.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly JsonSerializerSettings _serializerSettings = CreateSettings();

        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key)
        {
            var json = await cache.GetStringAsync(key);

            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        public static async Task SetObjectAsync(this IDistributedCache cache, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value, _serializerSettings);
            await cache.SetStringAsync(key, json);
        }

        public static async Task SetObjectAsync(
            this IDistributedCache cache,
            string key,
            object value,
            DistributedCacheEntryOptions options)
        {
            var json = JsonConvert.SerializeObject(value, _serializerSettings);
            await cache.SetStringAsync(key, json, options);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings();
        }
    }
}
