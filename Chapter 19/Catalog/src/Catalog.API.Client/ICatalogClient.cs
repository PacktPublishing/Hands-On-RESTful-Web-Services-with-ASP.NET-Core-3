using Catalog.API.Client.Resources;

namespace Catalog.API.Client
{
    public interface ICatalogClient
    {
        ICatalogItemResource Item { get; }
    }
}