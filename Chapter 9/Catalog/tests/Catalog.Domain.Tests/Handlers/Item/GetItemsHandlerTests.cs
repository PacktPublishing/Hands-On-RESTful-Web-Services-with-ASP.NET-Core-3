using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Commands.Item;
using Catalog.Domain.Handlers.Item;
using Catalog.Domain.Infrastructure.Mapper;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Handlers.Item
{
    public class GetItemsHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GetItemsHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        [Fact]
        public async Task getitems_should_return_right_data()
        {
            var sut = new GetItemsHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.Handle(new GetItemsCommand(), CancellationToken.None);

            result.Count.ShouldBe(4);
        }
    }
}