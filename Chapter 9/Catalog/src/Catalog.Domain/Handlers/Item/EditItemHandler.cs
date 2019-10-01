using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Repositories;
using Catalog.Domain.Responses.Item;
using MediatR;

namespace Catalog.Domain.Handlers.Item
{
    public class EditItemHandler : IRequestHandler<EditItemRequest, ItemResponse>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public EditItemHandler(IItemRepository itemsRepository, IMapper mapper)
        {
            _itemRepository = itemsRepository;
            _mapper = mapper;
        }

        public async Task<ItemResponse> Handle(EditItemRequest request, CancellationToken cancellationToken)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);

            if (existingRecord == null)
            {
                throw new ArgumentException($"Entity with {request.Id} is not present");
            }

            var entity = _mapper.Map<Entities.Item>(request);
            var result = _itemRepository.Update(entity);

            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ItemResponse>(result);
        }
    }
}