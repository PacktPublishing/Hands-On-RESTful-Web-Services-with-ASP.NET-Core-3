using System.Net.Http;
using Catalog.API.Client.Base;
using Catalog.API.Client.Resources;

namespace Catalog.API.Client
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