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
        public async Task should_return_record_by_id(Artist request)
        {
            var sut = new ArtistRepository(_testDataContextFactory.ContextInstance);

            var result = await sut.GetAsync(request.ArtistId);

            result.ArtistId.ShouldBe(request.ArtistId);
            result.ArtistName.ShouldBe(request.ArtistName);
        }


        [Theory]
        [LoadTestData("record-test.json", "artist_with_id")]
        public async Task should_add_new_item(Artist request)
        {
            request.ArtistId = Guid.NewGuid();
            var sut = new ArtistRepository(_testDataContextFactory.ContextInstance);
            sut.Add(request);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _testDataContextFactory.ContextInstance.Artists
                .FirstOrDefault(x => x.ArtistId == request.ArtistId)
                .ShouldNotBeNull();
        }
    }
}
