using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Item
{
    public class AddItemHandler : IRequestHandler<AddItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public AddItemHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
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
