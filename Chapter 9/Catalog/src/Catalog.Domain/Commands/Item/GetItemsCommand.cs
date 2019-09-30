using System.Collections.Generic;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Commands.Item
{
    public class GetItemsCommand : IRequest<IList<ItemResponse>>
    {
    }
}