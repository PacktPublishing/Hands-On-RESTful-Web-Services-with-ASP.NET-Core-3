using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Configurations;
using Catalog.Domain.Events;
using Catalog.Domain.Logging;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemMapper _itemMapper;
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<IItemService> _logger;
        private readonly ConnectionFactory _eventBusConnectionFactory;
        private readonly EventBusSettings _settings;


        public ItemService(IItemRepository itemRepository, IItemMapper itemMapper, ILogger<IItemService> logger, 
            ConnectionFactory eventBusConnectionFactory, EventBusSettings settings)
        {
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
            _logger = logger;
            _eventBusConnectionFactory = eventBusConnectionFactory;
            _settings = settings;
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var result = await _itemRepository.GetAsync();

            _logger.LogInformation(Logging.Events.GetById, Messages.NumberOfRecordAffected_modifiedRecords,
                result.Count());

            return result
                .Select(x => _itemMapper.Map(x));
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var entity = await _itemRepository.GetAsync(request.Id);

            _logger.LogInformation(Logging.Events.GetById, Messages.TargetEntityChanged_id, entity?.Id);

            return _itemMapper.Map(entity);
        }

        public async Task<ItemResponse> AddItemAsync(AddItemRequest request, CancellationToken cancellationToken)
        {
            var item = _itemMapper.Map(request);

            var result = _itemRepository.Add(item);
            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(Logging.Events.Add, Messages.NumberOfRecordAffected_modifiedRecords, modifiedRecords);
            _logger.LogInformation(Logging.Events.Add, Messages.ChangesApplied_id, result?.Id);

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request, CancellationToken cancellationToken)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);

            if (existingRecord == null) throw new ArgumentException($"Entity with {request.Id} is not present");

            var entity = _itemMapper.Map(request);
            var result = _itemRepository.Update(entity);

            var modifiedRecords = await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(Logging.Events.Edit, Messages.NumberOfRecordAffected_modifiedRecords,
                modifiedRecords);
            _logger.LogInformation(Logging.Events.Edit, Messages.ChangesApplied_id, result?.Id);

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(request.Id);
            result.IsInactive = false;

            _itemRepository.Update(result);
            await _itemRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            SendDeleteMessage(new ItemSoldOutEvent { Id = request.Id.ToString() });
            return _itemMapper.Map(result);
        }

        private void SendDeleteMessage(ItemSoldOutEvent message)
        {
            try
            {
                var connection = _eventBusConnectionFactory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _settings.EventQueue, true, false);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _settings.EventQueue, body: body);
                channel.WaitForConfirmsOrDie();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to initialize the event bus: {message}", e.Message);
            }
        }
    }
}