using VinylStore.Catalog.API.Client.Resources;

namespace VinylStore.Catalog.API.Client
{
    public interface ICatalogClient
    {
        ICatalogItemResource Item { get; }
    }
}