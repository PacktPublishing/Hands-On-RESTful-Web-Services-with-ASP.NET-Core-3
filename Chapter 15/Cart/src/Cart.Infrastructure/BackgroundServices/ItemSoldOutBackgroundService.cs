using System;
using System.Threading;
using System.Threading.Tasks;
using Cart.Domain.Events;
using Cart.Infrastructure.Configurations;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.Infrastructure.BackgroundServices
{
    public class ItemSoldOutBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ItemSoldOutBackgroundService> _logger;
        private readonly EventBusSettings _settings;
        private readonly IModel _channel;

        public ItemSoldOutBackgroundService(IMediator mediator,
            EventBusSettings settings, ConnectionFactory factory, ILogger<ItemSoldOutBackgroundService> logger)
        {
            _settings = settings;
            _logger = logger;
            _mediator = mediator;

            try
            {
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to initialize the event bus: {message}", e.Message);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += async (ch, ea) =>
            {
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);
                var @event = JsonConvert.DeserializeObject<ItemSoldOutEvent>(content);

                _logger.LogInformation("Consuming the following message from the event bus: {message}",
                    JsonConvert.SerializeObject(@event));
                
                await _mediator.Send(@event, stoppingToken);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            try
            {
                consumer.Model.QueueDeclare(_settings.EventQueue, true, false);
                _channel.BasicConsume(_settings.EventQueue, false, consumer);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to consume the event bus: {message}", e.Message);
            }

            return Task.CompletedTask;
        }
    }
}