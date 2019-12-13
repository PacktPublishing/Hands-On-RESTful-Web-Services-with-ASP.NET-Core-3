using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Repositories;
using Cart.Events;
using Cart.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.Infrastructure.BackgroundServices
{
    public class ItemSoldOutBackgroundService : BackgroundService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<ItemSoldOutBackgroundService> _logger;
        private readonly EventBusSettings _options;

        private IModel _channel;
        private IConnection _connection;

        public ItemSoldOutBackgroundService(ICartRepository cartRepository,
            IOptions<EventBusSettings> options, ILogger<ItemSoldOutBackgroundService> logger)
        {
            _cartRepository = cartRepository;
            _options = options.Value;
            _logger = logger;

            InitBus();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                OnSoldoutMessageReceived(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            try
            {
                _channel.BasicConsume(_options.EventQueue, false, consumer);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to consume the event bus: {message}", e.Message);
            }

            return Task.CompletedTask;
        }

        private void OnSoldoutMessageReceived(string message)
        {
            ItemSoldOutEvent @event = JsonConvert.DeserializeObject<ItemSoldOutEvent>(message);

            _logger.LogInformation($"Message recieved: {JsonConvert.SerializeObject(@event)}");

            var cartIds = _cartRepository.GetCarts().ToList();

            var tasks = cartIds.Select(async x =>
            {
                var cart = await _cartRepository.GetAsync(new Guid(x));
                await RemoveItemsInCart(@event.Id, cart);
            });

            Task.WhenAll(tasks);
        }

        private async Task RemoveItemsInCart(string itemToRemove, Domain.Entities.Cart cart)
        {
            if (string.IsNullOrEmpty(itemToRemove)) return;

            var toDelete = cart?.Items?.Where(x => x.CartItemId.ToString() == itemToRemove).ToList();

            if (toDelete == null || toDelete.Count == 0) return;

            foreach (var item in toDelete) cart.Items?.Remove(item);

            await _cartRepository.AddOrUpdateAsync(cart);
        }

        private void InitBus()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    UserName = _options.User,
                    Password = _options.Password
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to initialize the event bus: {message}", e.Message);
            }
        }
    }
}