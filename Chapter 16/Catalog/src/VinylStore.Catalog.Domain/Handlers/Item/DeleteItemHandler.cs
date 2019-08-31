using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Item
{
    public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public DeleteItemHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ItemResponse> Handle(DeleteItemCommand command,
            CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(command.Id);
            result.IsInactive = false;

            _itemRepository.Update(result);
            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ItemResponse>(result);
        }
    }
}
