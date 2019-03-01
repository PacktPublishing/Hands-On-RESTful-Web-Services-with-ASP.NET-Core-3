using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VinylStore.Catalog.API.Client.Base
{
    public class BaseClient : IBaseClient
    {
        private readonly string _baseUri;
        private readonly HttpClient _client;

        public BaseClient(HttpClient client, string baseUri)
        {
            _client = client;
            _baseUri = baseUri;
        }

        public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken)
        {
            var result = await _client.GetAsync(uri, cancellationToken);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public Uri BuildUri(string format)
        {
            return new UriBuilder(_baseUri)
            {
                Path = format
            }.Uri;
        }
    }
}