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
    public class EditItemHandler : IRequestHandler<EditItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public EditItemHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }


        public async Task<ItemResponse> Handle(EditItemCommand command, CancellationToken cancellationToken)
        {
            var existingRecord = await _itemRepository.GetAsync(command.Id);

            if (existingRecord == null)
            {
                throw new ArgumentException($"Entity with {command.Id} is not present");
            }

            var entity = _mapper.Map<Entities.Item>(command);
            var result = _itemRepository.Update(entity);

            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ItemResponse>(result);
        }
    }
}