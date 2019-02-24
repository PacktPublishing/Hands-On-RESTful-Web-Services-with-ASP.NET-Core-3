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
        public async Task should_return_record_by_id(object jsonPayload)
        {
            var genre = JsonConvert.DeserializeObject<Genre>(jsonPayload.ToString());
            var sut = new GenreRepository(_testDataContextFactory.ContextInstance);

            var result = await sut.GetAsync(genre.GenreId);

            result.GenreId.ShouldBe(genre.GenreId);
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }


        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task should_add_new_item(object jsonPayload)
        {
            var genre = JsonConvert.DeserializeObject<Genre>(jsonPayload.ToString());
            genre.GenreId=Guid.NewGuid();
            
            var sut = new GenreRepository(_testDataContextFactory.ContextInstance);
            sut.Add(genre);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _testDataContextFactory.ContextInstance.Genre
                .FirstOrDefault(x => x.GenreId == genre.GenreId)
                .ShouldNotBeNull();
        }

    }
}

