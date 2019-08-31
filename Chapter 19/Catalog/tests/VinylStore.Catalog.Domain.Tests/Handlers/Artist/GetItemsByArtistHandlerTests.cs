using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Handlers.Artist;
using VinylStore.Catalog.Domain.Infrastructure.Mapper;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Artist
{
    public class GetItemsByArtistHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GetItemsByArtistHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }


        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task handle_should_return_right_items_using_artist_id(string id)
        {
            var repository = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetItemsByArtistHandler(repository,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result = await sut.Handle(new PaginatedItemsResponseModel { Id = new Guid(id) }, CancellationToken.None);

            result.ShouldNotBeNull();
        }
    }
}
