using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Configurations;
using Catalog.Domain.Entities;
using Catalog.Domain.Mappers;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Domain.Tests.Services
{
    public class ItemServiceTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _itemRepository;
        private readonly IItemMapper _mapper;
        private readonly Mock<LoggerAbstraction<IItemService>> _logger;

        public ItemServiceTests(CatalogContextFactory catalogContextFactory, ITestOutputHelper testOutputHelper)
        {
            _itemRepository = new ItemRepository(catalogContextFactory.ContextInstance);
            _mapper = catalogContextFactory.ItemMapper;

            _logger = new Mock<LoggerAbstraction<IItemService>>();
            _logger
                .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()))
                .Callback((LogLevel logLevel, Exception exception, string information) => testOutputHelper.WriteLine($"{logLevel}:{information}"));
        }

        [Theory]
        [LoadData("item")]
        public async Task additem_should_add_right_entity(AddItemRequest request)
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());

            var result =
                await sut.AddItemAsync(request);

            result.Name.ShouldBe(request.Name);
            result.Description.ShouldBe(request.Description);
            result.GenreId.ShouldBe(request.GenreId);
            result.ArtistId.ShouldBe(request.ArtistId);
            result.Price.Amount.ShouldBe(request.Price.Amount);
            result.Price.Currency.ShouldBe(request.Price.Currency);
        }

        [Theory]
        [LoadData("item")]
        public async Task additem_should_log_information(AddItemRequest request)
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());
            await sut.AddItemAsync(request);

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(2));
        }

        [Theory]
        [LoadData("item")]
        public async Task edititem_should_add_right_entity(EditItemRequest request)
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());

            var result =
                await sut.EditItemAsync(request);

            result.Name.ShouldBe(request.Name);
            result.Description.ShouldBe(request.Description);
            result.GenreId.ShouldBe(request.GenreId);
            result.ArtistId.ShouldBe(request.ArtistId);
            result.Price.Amount.ShouldBe(request.Price.Amount);
            result.Price.Currency.ShouldBe(request.Price.Currency);
        }

        [Fact]
        public void getitem_should_thrown_exception_with_null_id()
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());
            sut.GetItemAsync(null).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task getitems_should_return_right_data()
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());

            var result =
                await sut.GetItemsAsync();

            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getitem_should_log_right_information(string guid)
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());

            await sut.GetItemAsync(new GetItemRequest { Id = new Guid(guid) });

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(1));
        }

        [Fact]
        public async Task getitems_should_log_right_information()
        {
            var sut = new ItemService(_itemRepository, _mapper, _logger.Object, new ConnectionFactory(), new EventBusSettings());

            await sut.GetItemsAsync();

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(1));
        }

    }
}