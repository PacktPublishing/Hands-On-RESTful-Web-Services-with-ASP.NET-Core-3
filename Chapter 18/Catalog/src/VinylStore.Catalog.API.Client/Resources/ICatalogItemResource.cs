using System;
using System.Threading;
using System.Threading.Tasks;
using VinylStore.Catalog.API.Contract.Item;

namespace VinylStore.Catalog.API.Client.Resources
{
    public interface ICatalogItemResource
    {
        Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
