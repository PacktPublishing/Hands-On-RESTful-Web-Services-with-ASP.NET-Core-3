using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Item;
using VinylStore.Catalog.Domain.Handlers.Item;
using VinylStore.Catalog.Domain.Infrastructure.Mapper;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Item
{
    public class EditItemHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;
        private readonly Mock<LoggerAbstraction<EditItemHandler>> _logger;


        public EditItemHandlerTests(CatalogDataContextFactory catalogDataContextFactory, ITestOutputHelper output)
        {
            _catalogDataContextFactory = catalogDataContextFactory;

            _logger = new Mock<LoggerAbstraction<EditItemHandler>>();
            _logger
                .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()))
                .Callback((LogLevel logLevel, Exception exception, string information) =>
                    output.WriteLine($"{logLevel}:{information}"));
        }


        [Theory]
        [InlineData(
            "{\"Id\":\"b5b05534-9263-448c-a69e-0bbd8b3eb90e\", \"Name\":\"Test album\",\"Description\":\"Description\",\"LabelName\":\"LabelName\",\"Price\":{\"Amount\":23.5,\"Currency\":\"EUR\"},\"PictureUri\":\"https://mycdn.com/pictures/32423423\",\"ReleaseDate\":\"2016-01-01T00:00:00+00:00\",\"Format\":\"Vinyl 33g\",\"AvailableStock\":6,\"GenreId\":\"c04f05c0-f6ad-44d1-a400-3375bfb5dfd6\",\"Genre\":null,\"ArtistId\":\"f08a333d-30db-4dd1-b8ba-3b0473c7cdab\",\"Artist\":null}")]
        public async Task edititem_should_add_right_entity(string json)
        {
            var item = JsonConvert.DeserializeObject<EditItemCommand>(json);

            var sut = new EditItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);

            var result =
                await sut.Handle(item, CancellationToken.None);

            result.Name.ShouldBe(item.Name);
            result.Description.ShouldBe(item.Description);
            result.GenreId.ShouldBe(item.GenreId);
            result.ArtistId.ShouldBe(item.ArtistId);
            result.Price.Amount.ShouldBe(item.Price.Amount);
            result.Price.Currency.ShouldBe(item.Price.Currency);

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(2));
        }
    }
}
