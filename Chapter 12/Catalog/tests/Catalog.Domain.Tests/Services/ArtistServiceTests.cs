using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Services
{
    public class ArtistServiceTests : IClassFixture<CatalogContextFactory>
    {
        public ArtistServiceTests(CatalogContextFactory catalogContextFactory)
        {
            _catalogContextFactory = catalogContextFactory;
        }

        private readonly CatalogContextFactory _catalogContextFactory;

        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task getartist_should_return_right_artist(string id)
        {
            var artistRepository = new ArtistRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new ArtistService(artistRepository, itemRepository,
                _catalogContextFactory.ArtistMapper, _catalogContextFactory.ItemMapper);

            var result = await sut.GetArtistAsync(new GetArtistRequest { Id = new Guid(id) });
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task handle_should_return_right_items_using_artist_id(string id)
        {
            var artistRepository = new ArtistRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new ArtistService(artistRepository, itemRepository,
                _catalogContextFactory.ArtistMapper, _catalogContextFactory.ItemMapper);

            var result = await sut.GetItemByArtistIdAsync(new GetArtistRequest { Id = new Guid(id) });
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("{\"ArtistName\":\"TestArtist\"}")]
        public async Task addartist_should_add_right_artist(string json)
        {
            var artistRepository = new ArtistRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new ArtistService(artistRepository, itemRepository,
                _catalogContextFactory.ArtistMapper, _catalogContextFactory.ItemMapper);

            var artist = JsonConvert.DeserializeObject<AddArtistRequest>(json);

            var result =
                await sut.AddArtistAsync(artist, CancellationToken.None);

            result.ArtistId.ShouldNotBeNull();
            result.ArtistName.ShouldBe(artist.ArtistName);
        }

        [Fact]
        public async Task getartist_should_return_right_data()
        {
            var artistRepository = new ArtistRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new ArtistService(artistRepository, itemRepository,
                _catalogContextFactory.ArtistMapper, _catalogContextFactory.ItemMapper);

            var result =
                await sut.GetArtistsAsync();
            result
                .ToList().Count.ShouldBe(2);
        }

        [Fact]
        public void getartist_should_thrown_exception_with_null_id()
        {
            var artistRepository = new ArtistRepository(_catalogContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogContextFactory.ContextInstance);

            var sut = new ArtistService(artistRepository, itemRepository,
                _catalogContextFactory.ArtistMapper, _catalogContextFactory.ItemMapper);

            sut.GetArtistAsync(null).ShouldThrow<ArgumentNullException>();
        }
    }
}