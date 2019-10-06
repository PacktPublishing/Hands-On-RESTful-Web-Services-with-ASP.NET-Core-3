using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Mapper;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Handlers
{
    public class GenreServiceTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly IGenreService _sut;
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GenreServiceTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;

            var genreRepository = new GenreRepository(_catalogDataContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            _sut = new GenreService(genreRepository, itemRepository,
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));
        }

        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task getgenre_should_return_right_genre(string id)
        {
            var result = await _sut.GetGenreAsync(new GetGenreRequest { Id = new Guid(id) }, CancellationToken.None);
            result.ShouldNotBeNull();
        }

        [Fact]
        public void getgenre_should_thrown_exception_with_null_id()
        {
            _sut.GetGenreAsync(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task get_multiple_genre_should_return_right_data()
        {
            var result = await _sut.GetGenreAsync(CancellationToken.None);
            result
                .ToList().Count.ShouldBe(1);
        }

        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task handle_should_return_right_items_using_genre_id(string id)
        {
            var result = await _sut.GetItemByGenreIdAsync(new GetItemsByGenreRequest { Id = new Guid(id) }, CancellationToken.None);
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("{\"GenreDescription\": \"Jazz\"}")]
        public async Task addgenre_should_add_right_genre(string json)
        {
            _catalogDataContextFactory.ContextInstance.Database.EnsureDeleted();

            var genre = JsonConvert.DeserializeObject<AddGenreRequest>(json);

            var genreRepository = new GenreRepository(_catalogDataContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GenreService(genreRepository, itemRepository,
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.AddGenreAsync(genre, CancellationToken.None);

            result.GenreId.ShouldNotBeNull();
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }
    }
}