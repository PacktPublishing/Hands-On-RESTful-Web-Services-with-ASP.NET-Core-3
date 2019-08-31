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
    public class GetItemHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;
        private readonly Mock<LoggerAbstraction<GetItemHandler>> _logger;


        public GetItemHandlerTests(CatalogDataContextFactory catalogDataContextFactory, ITestOutputHelper output)
        {
            _catalogDataContextFactory = catalogDataContextFactory;

            _logger = new Mock<LoggerAbstraction<GetItemHandler>>();

            _logger
                .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()))
                .Callback((LogLevel logLevel, Exception exception, string information) =>
                    output.WriteLine($"{logLevel}:{information}"));
        }


        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getitem_should_return_right_data(string guid)
        {
            var sut = new GetItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);

            var result =
                await sut.Handle(new GetItemCommand { Id = new Guid(guid) }, CancellationToken.None);

            result.Id.ShouldBe(new Guid(guid));
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getitem_should_log_right_information(string guid)
        {
            var sut = new GetItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);

            var result =
                await sut.Handle(new GetItemCommand { Id = new Guid(guid) }, CancellationToken.None);

            _logger
                .Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.AtMost(1));
        }

        [Fact]
        public void getitem_should_thrown_exception_with_null_id()
        {
            var sut = new GetItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())), _logger.Object);
            sut.Handle(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }
    }
}
