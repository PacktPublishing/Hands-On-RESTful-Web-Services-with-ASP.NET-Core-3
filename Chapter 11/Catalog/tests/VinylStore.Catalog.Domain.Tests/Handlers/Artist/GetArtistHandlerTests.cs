using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Handlers.Artist;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Artist
{
    public class GetArtistHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        public GetArtistHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;


        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task getartist_should_return_right_artist(string id)
        {
            var repository = new ArtistRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetArtistHandler(repository);

            var result = await sut.Handle(new GetArtistCommand { Id = new Guid(id) }, CancellationToken.None);

            result.ShouldNotBeNull();
        }

        [Fact]
        public void getartist_should_thrown_exception_with_null_id()
        {
            var repository = new ArtistRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetArtistHandler(repository);

            sut.Handle(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }
    }
}