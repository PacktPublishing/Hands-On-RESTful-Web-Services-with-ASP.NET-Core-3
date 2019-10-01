using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<IList<ItemResponse>> GetItems(CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetAsync();
            return _mapper.Map<IList<ItemResponse>>(result);
        }

        public async Task<ItemResponse> GetItem(GetItemRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            return _mapper.Map<ItemResponse>(await _itemRepository.GetAsync(request.Id));
        }

        public async Task<ItemResponse> AddItem(AddItemRequest request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Entities.Item>(request);

            var result = _itemRepository.Add(item);
            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> EditItem(EditItemRequest request, CancellationToken cancellationToken)
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