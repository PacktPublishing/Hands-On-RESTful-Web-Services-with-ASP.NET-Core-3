using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Services
{
    public class GenreServiceTests : IClassFixture<CatalogContextFactory>
    {
        public GenreServiceTests(CatalogContextFactory catalogContextFactory)
        {
            _catalogContextFactory = catalogContextFactory;
        }

        private readonly CatalogContextFactory _catalogContextFactory;

        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task getgenre_should_return_right_genre(string id)
        {
            var genreRepository = new GenreRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                _catalogContextFactory.GenreMapper, _catalogContextFactory.ItemMapper);

            var result = await sut.GetGenreAsync(new GetGenreRequest { Id = new Guid(id) });
            result.GenreId.ShouldBe(new Guid(id));
        }

        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task handle_should_return_right_items_using_genre_id(string id)
        {
            var genreRepository = new GenreRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                _catalogContextFactory.GenreMapper, _catalogContextFactory.ItemMapper);

            var result = await sut.GetItemByGenreIdAsync(new GetGenreRequest { Id = new Guid(id) });
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("{\"GenreDescription\": \"Jazz\"}")]
        public async Task addgenre_should_add_right_genre(string json)
        {
            var genre = JsonConvert.DeserializeObject<AddGenreRequest>(json);

            var genreRepository = new GenreRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                _catalogContextFactory.GenreMapper, _catalogContextFactory.ItemMapper);

            var result =
                await sut.AddGenreAsync(genre, CancellationToken.None);

            result.GenreId.ShouldNotBeNull();
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }

        [Fact]
        public async Task get_multiple_genre_should_return_right_data()
        {
            var genreRepository = new GenreRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                _catalogContextFactory.GenreMapper, _catalogContextFactory.ItemMapper);

            var result = await sut.GetGenreAsync();
            result.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public void getgenre_should_thrown_exception_with_null_id()
        {
            var genreRepository = new GenreRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                _catalogContextFactory.GenreMapper, _catalogContextFactory.ItemMapper);

            sut.GetGenreAsync(null).ShouldThrow<ArgumentNullException>();
        }
    }
}