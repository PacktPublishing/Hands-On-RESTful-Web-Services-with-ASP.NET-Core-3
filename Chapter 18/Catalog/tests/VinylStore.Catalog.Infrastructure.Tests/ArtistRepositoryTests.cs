using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shouldly;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Infrastructure.Tests
{
    public class ArtistRepositoryTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _testDataContextFactory;


        public ArtistRepositoryTests(CatalogDataContextFactory testDataContextFactory)
        {
            _testDataContextFactory = testDataContextFactory;
        }

        [Theory]
        [LoadTestData("record-test.json", "artist_with_id")]
        public async Task should_return_record_by_id(object jsonPayload)
        {
            var artist = JsonConvert.DeserializeObject<Artist>(jsonPayload.ToString());
            var sut = new ArtistRepository(_testDataContextFactory.ContextInstance);

            var result = await sut.GetAsync(artist.ArtistId);

            result.ArtistId.ShouldBe(artist.ArtistId);
            result.ArtistName.ShouldBe(artist.ArtistName);
        }


        [Theory]
        [LoadTestData("record-test.json", "artist_with_id")]
        public async Task should_add_new_item(object jsonPayload)
        {
            var artist = JsonConvert.DeserializeObject<Artist>(jsonPayload.ToString());
            artist.ArtistId = Guid.NewGuid();

            var sut = new ArtistRepository(_testDataContextFactory.ContextInstance);
            sut.Add(artist);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _testDataContextFactory.ContextInstance.Artists
                .FirstOrDefault(x => x.ArtistId == artist.ArtistId)
                .ShouldNotBeNull();
        }
    }
}
