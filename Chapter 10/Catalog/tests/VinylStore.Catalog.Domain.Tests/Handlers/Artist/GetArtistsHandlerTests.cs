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
    public class GetArtistsHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        public GetArtistsHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        [Fact]
        public async Task getartist_should_return_right_data()
        {
            var repository = new ArtistRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetArtistsHandler(repository,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.Handle(new GetArtistsCommand(), CancellationToken.None);

            result.Count.ShouldBe(2);
        }
    }
}