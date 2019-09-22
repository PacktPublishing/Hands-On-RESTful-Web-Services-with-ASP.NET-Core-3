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
    public class GenreRepositoryTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _testDataContextFactory;


        public GenreRepositoryTests(CatalogDataContextFactory testDataContextFactory)
        {
            _testDataContextFactory = testDataContextFactory;
        }

        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task should_return_record_by_id(Genre request)
        {
            var sut = new GenreRepository(_testDataContextFactory.ContextInstance);
            var result = await sut.GetAsync(request.GenreId);

            result.GenreId.ShouldBe(request.GenreId);
            result.GenreDescription.ShouldBe(request.GenreDescription);
        }


        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task should_add_new_item(Genre request)
        {
            request.GenreId = Guid.NewGuid();

            var sut = new GenreRepository(_testDataContextFactory.ContextInstance);
            sut.Add(request);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _testDataContextFactory.ContextInstance.Genres
                .FirstOrDefault(x => x.GenreId == request.GenreId)
                .ShouldNotBeNull();
        }
    }
}
