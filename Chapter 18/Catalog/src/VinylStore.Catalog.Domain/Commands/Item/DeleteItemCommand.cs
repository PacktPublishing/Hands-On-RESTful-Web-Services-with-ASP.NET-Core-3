using System;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Item
{
    public class DeleteItemCommand : IRequest<ItemResponse>
    {
        public Guid Id { get; set; }
    }
}
