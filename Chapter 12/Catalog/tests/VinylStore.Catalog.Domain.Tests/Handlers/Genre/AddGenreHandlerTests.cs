using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Handlers.Genre;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Genre
{
    public class AddGenreHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public AddGenreHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        [Theory]
        [InlineData("{\"GenreDescription\": \"Jazz\"}")]
        public async Task addgenre_should_add_right_genre(string json)
        {
            _catalogDataContextFactory.ContextInstance.Database.EnsureDeleted();

            var genre = JsonConvert.DeserializeObject<AddGenreCommand>(json);

            var repository = new GenreRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new AddGenreHandler(repository);

            var result =
                await sut.Handle(genre, CancellationToken.None);

            result.GenreId.ShouldNotBeNull();
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }
    }
}
