using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Contract.Item;

namespace Catalog.API.Client.Resources
{
    public interface ICatalogItemResource
    {
        Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default);
    }
}