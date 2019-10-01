using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Repositories;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Handlers.Item
{
    public class AddItemHandler : IRequestHandler<AddItemRequest, ItemResponse>
    {
      
        public AddItemHandler(IItemRepository itemsRepository, IMapper mapper)
        {
            _itemRepository = itemsRepository;
            _mapper = mapper;
        }

       
    }
}