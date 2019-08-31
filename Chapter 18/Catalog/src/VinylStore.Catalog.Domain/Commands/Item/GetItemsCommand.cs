using System.Collections.Generic;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Item
{
    public class GetItemsCommand : IRequest<IList<ItemResponse>>
    {
    }
}
