using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Mapper;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Handlers
{
    public class ArtistServiceTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly IArtistService _sut;
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public ArtistServiceTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;

            var artistRepository = new ArtistRepository(_catalogDataContextFactory.ContextInstance);
            var itemRepository = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            _sut = new ArtistService(artistRepository, itemRepository,
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));
        }

        [Fact]
        public async Task getartist_should_return_right_data()
        {
            var result =
                await _sut.GetArtistsAsync(CancellationToken.None);
            result
                .ToList().Count.ShouldBe(3);
        }

        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task getartist_should_return_right_artist(string id)
        {
            var result = await _sut.GetArtistAsync(new GetArtistRequest { Id = new Guid(id) }, CancellationToken.None);
            result.ShouldNotBeNull();
        }

        [Fact]
        public void getartist_should_thrown_exception_with_null_id()
        {
            _sut.GetArtistAsync(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [InlineData("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")]
        public async Task handle_should_return_right_items_using_artist_id(string id)
        {
            var result = await _sut.GetItemByArtistIdAsync(new GetItemsByArtistRequest { Id = new Guid(id) }, CancellationToken.None);
            result.ShouldNotBeNull();
        }
        [Theory]
        [InlineData("{\"ArtistName\":\"TestArtist\"}")]
        public async Task addartist_should_add_right_artist(string json)
        {
            var artist = JsonConvert.DeserializeObject<AddArtistRequest>(json);

            var result =
                await _sut.AddArtist(artist, CancellationToken.None);

            result.ArtistId.ShouldNotBeNull();
            result.ArtistName.ShouldBe(artist.ArtistName);
        }
    }
}