using System;
using System.Collections.Generic;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Item
{
    public class GetItemCommand : IRequest<ItemResponse>
    {
        public Guid Id { get; set; }
    }
}