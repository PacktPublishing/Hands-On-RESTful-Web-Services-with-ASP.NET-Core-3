using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Logging;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemMapper _itemMapper;
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<IItemService> _logger;

        public ItemService(IItemRepository itemRepository, IItemMapper itemMapper, ILogger<IItemService> logger)
        {
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var result = await _itemRepository.GetAsync();

            _logger.LogInformation(Events.GetById, Messages.NumberOfRecordAffected_modifiedRecords,
                result.Count);

            return result
                .Select(x => _itemMapper.Map(x));
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var entity = await _itemRepository.GetAsync(request.Id);

            _logger.LogInformation(Events.GetById, Messages.TargetEntityChanged_id, entity?.Id);

            return _itemMapper.Map(entity);
        }

        public async Task<ItemResponse> AddItemAsync(AddItemRequest request, CancellationToken cancellationToken)
        {
            var item = _itemMapper.Map(request);

            var result = _itemRepository.Add(item);
            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(Events.Add, Messages.NumberOfRecordAffected_modifiedRecords, modifiedRecords);
            _logger.LogInformation(Events.Add, Messages.ChangesApplied_id, result?.Id);

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request, CancellationToken cancellationToken)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);

            if (existingRecord == null) throw new ArgumentException($"Entity with {request.Id} is not present");

            var entity = _itemMapper.Map(request);
            var result = _itemRepository.Update(entity);

            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(Events.Edit, Messages.NumberOfRecordAffected_modifiedRecords,
                modifiedRecords);
            _logger.LogInformation(Events.Edit, Messages.ChangesApplied_id, result?.Id);

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(request.Id);
            result.IsInactive = false;

            _itemRepository.Update(result);
            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(Events.Delete, Messages.NumberOfRecordAffected_modifiedRecords,
                modifiedRecords);

            return _itemMapper.Map(result);
        }
    }
}