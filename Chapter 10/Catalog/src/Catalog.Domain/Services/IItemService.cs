using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public interface IItemService
    {
        Task<IList<ItemResponse>> GetItems(CancellationToken cancellationToken);
        Task<ItemResponse> GetItem(GetItemRequest request, CancellationToken cancellationToken);
        Task<ItemResponse> AddItem(AddItemRequest request, CancellationToken cancellationToken);
        Task<ItemResponse> EditItem(EditItemRequest request, CancellationToken cancellationToken);
    }
}