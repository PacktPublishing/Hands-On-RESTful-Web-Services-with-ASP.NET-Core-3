using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Infrastructure.Repositories;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Handlers.Item
{
    public class EditItemHandler : IRequestHandler<EditItemCommand, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public EditItemHandler(IItemRepository itemsRepository, IMapper mapper)
        {
            _itemRepository = itemsRepository;
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