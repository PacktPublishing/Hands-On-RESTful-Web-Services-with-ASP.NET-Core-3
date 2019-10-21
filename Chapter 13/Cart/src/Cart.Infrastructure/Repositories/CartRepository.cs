using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cart.Domain.Infrastructure.Repositories;
using Cart.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cart.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        private readonly CartDataSourceSettings _settings;

        public CartRepository(IOptions<CartDataSourceSettings> options)
        {
            _settings = options.Value;
            var configuration = ConfigurationOptions.Parse(_settings.RedisConnectionString);

            try
            {
                _database = ConnectionMultiplexer.Connect(configuration).GetDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public IEnumerable<string> GetCarts()
        {
            var keys = _database.Multiplexer.GetServer(_settings.RedisConnectionString).Keys();
            return keys?.Select(k => k.ToString());
        }


        public async Task<Domain.Entities.Cart> GetAsync(Guid id)
        {
            var data = await _database.StringGetAsync(id.ToString());
            return data.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Domain.Entities.Cart>(data);
        }

        public async Task<Domain.Entities.Cart> AddOrUpdateAsync(Domain.Entities.Cart item)
        {
            var created = await _database.StringSetAsync(item.Id, JsonConvert.SerializeObject(item));
            if (!created) return null;

            return await GetAsync(new Guid(item.Id));
        }
    }
}