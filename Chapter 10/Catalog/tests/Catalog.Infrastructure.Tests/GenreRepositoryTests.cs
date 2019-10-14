using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using Xunit;

namespace Catalog.Infrastructure.Tests
{
    public class GenreRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly CatalogContextFactory _testContextFactory;


        public GenreRepositoryTests(CatalogContextFactory testContextFactory)
        {
            _testContextFactory = testContextFactory;
        }

        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task should_return_record_by_id(Genre genre)
        {
            var sut = new GenreRepository(_testContextFactory.ContextInstance);

            var result = await sut.GetAsync(genre.GenreId);

            result.GenreId.ShouldBe(genre.GenreId);
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }


        [Theory]
        [LoadTestData("record-test.json", "genre_with_id")]
        public async Task should_add_new_item(Genre genre)
        {
            genre.GenreId = Guid.NewGuid();

            var sut = new GenreRepository(_testContextFactory.ContextInstance);
            sut.Add(genre);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _testContextFactory.ContextInstance.Genres
                .FirstOrDefault(x => x.GenreId == genre.GenreId)
                .ShouldNotBeNull();
        }

    }
}

