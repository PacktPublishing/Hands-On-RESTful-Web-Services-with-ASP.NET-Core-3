using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Infrastructure.Repositories;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Handlers.Item
{
    public class AddItemHandler : IRequestHandler<AddItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public AddItemHandler(IItemRepository itemsRepository, IMapper mapper)
        {
            _itemRepository = itemsRepository;
            _mapper = mapper;
        }

        public async Task<ItemResponse> Handle(AddItemCommand command, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Entities.Item>(command);

            var result = _itemRepository.Add(item);
            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ItemResponse>(result);
        }
    }
}