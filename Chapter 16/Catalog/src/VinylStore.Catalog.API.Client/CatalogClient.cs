using System.Net.Http;
using VinylStore.Catalog.API.Client.Base;
using VinylStore.Catalog.API.Client.Resources;

namespace VinylStore.Catalog.API.Client
{
    public class CatalogClient : ICatalogClient
    {
        public CatalogClient(HttpClient client)
        {
            Item = new CatalogItemResource(new BaseClient(client, client.BaseAddress.ToString()));
        }

        public ICatalogItemResource Item { get; }
    }
}
