using System;
using System.Collections.Generic;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Artists
{
    public class PaginatedItemsResponseModel : IRequest<IList<ItemResponse>>
    {
        public Guid Id { get; set; }
    }
}
