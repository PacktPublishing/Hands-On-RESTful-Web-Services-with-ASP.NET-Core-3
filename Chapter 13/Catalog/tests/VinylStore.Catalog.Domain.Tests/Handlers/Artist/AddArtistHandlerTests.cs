using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Artist;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Handlers.Artist;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Artist
{
    public class AddArtistHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public AddArtistHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        [Theory]
        [InlineData("{\"ArtistName\": \"The Braze\"}")]
        public async Task additem_should_add_right_artist(string json)
        {
            _catalogDataContextFactory.ContextInstance.Database.EnsureDeleted();

            var artist = JsonConvert.DeserializeObject<AddArtistCommand>(json);

            var repository = new ArtistRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new AddArtistHandler(repository);

            var result =
                await sut.Handle(artist, CancellationToken.None);

            result.ArtistId.ShouldNotBeNull();
            result.ArtistName.ShouldBe(artist.ArtistName);
        }
    }
}