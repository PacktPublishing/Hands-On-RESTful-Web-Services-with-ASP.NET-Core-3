using System;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Requests.Item
{
    public class GetItemRequest : IRequest<ItemResponse>
    {
        public Guid Id { get; set; }
    }
}