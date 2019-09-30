using System;
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
    public class GetItemHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GetItemHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getitem_should_return_right_data(string guid)
        {
            var sut = new GetItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.Handle(new GetItemCommand { Id = new Guid(guid) }, CancellationToken.None);

            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public void getitem_should_thrown_exception_with_null_id()
        {
            var sut = new GetItemHandler(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));
            sut.Handle(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }
    }
}