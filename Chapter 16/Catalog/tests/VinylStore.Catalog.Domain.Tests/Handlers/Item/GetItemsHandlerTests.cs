using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
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
    public class GetItemsHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly Mock<LoggerAbstraction<GetItemsHandler>> _logger;


        public GetItemsHandlerTests(CatalogDataContextFactory catalogDataContextFactory, ITestOutputHelper output)
        {
            _catalogDataContextFactory = catalogDataContextFactory;

            _logger = new Mock<LoggerAbstraction<GetItemsHandler>>();

            _logger
                .Setup(x => x.Log(It.IsAny<LogLevel>(), (Exception)It.IsAny<Exception>(), It.IsAny<string>()))
                .Callback((LogLevel logLevel, Exception exception, string information) =>
                    output.WriteLine($"{logLevel}:{information}"));
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        [Fact]
        public async Task getitems_should_return_right_data()
        {
            var sut = new GetItemsHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);

            var result =
                await sut.Handle(new GetItemsCommand(), CancellationToken.None);

            result.Count.ShouldBe(3);
        }

        [Fact]
        public async Task getitems_should_log_right_information()
        {
            var sut = new GetItemsHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);

            var result =
                await sut.Handle(new GetItemsCommand(), CancellationToken.None);

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(1));
        }
    }
}
