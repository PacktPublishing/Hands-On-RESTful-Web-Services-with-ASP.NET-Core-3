using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemMapper _itemMapper;
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository, IItemMapper itemMapper)
        {
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var result = await _itemRepository.GetAsync();
            return result
                .Select(x => _itemMapper.Map(x));
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var entity = await _itemRepository.GetAsync(request.Id);
            return _itemMapper.Map(entity);
        }

        public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
        {
            var item = _itemMapper.Map(request);

            var result = _itemRepository.Add(item);
            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);

            if (existingRecord == null) throw new ArgumentException($"Entity with {request.Id} is not present");

            var entity = _itemMapper.Map(request);
            var result = _itemRepository.Update(entity);

            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(request.Id);
            result.IsInactive = true;

            _itemRepository.Update(result);
            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
        }
    }
}