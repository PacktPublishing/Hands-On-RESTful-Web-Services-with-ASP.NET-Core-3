using System;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Commands.Item
{
    public class GetItemCommand : IRequest<ItemResponse>
    {
        public Guid Id { get; set; }
    }
}