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
        private readonly CatalogContextFactory _factory;

        public GenreRepositoryTests(CatalogContextFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [LoadData("genre")]
        public async Task should_return_record_by_id(Genre genre)
        {
            var sut = new GenreRepository(_factory.ContextInstance);

            var result = await sut.GetAsync(genre.GenreId);

            result.GenreId.ShouldBe(genre.GenreId);
            result.GenreDescription.ShouldBe(genre.GenreDescription);
        }


        [Theory]
        [LoadData("genre")]
        public async Task should_add_new_item(Genre genre)
        {
            genre.GenreId = Guid.NewGuid();

            var sut = new GenreRepository(_factory.ContextInstance);
            sut.Add(genre);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _factory.ContextInstance.Genres
                .FirstOrDefault(x => x.GenreId == genre.GenreId)
                .ShouldNotBeNull();
        }
    }
}

