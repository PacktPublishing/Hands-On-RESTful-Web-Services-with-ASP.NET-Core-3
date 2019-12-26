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
    public class ArtistRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly CatalogContextFactory _factory;

        public ArtistRepositoryTests(CatalogContextFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [LoadData("artist")]
        public async Task should_return_record_by_id(Artist artist)
        {
            var sut = new ArtistRepository(_factory.ContextInstance);

            var result = await sut.GetAsync(artist.ArtistId);

            result.ArtistId.ShouldBe(artist.ArtistId);
            result.ArtistName.ShouldBe(artist.ArtistName);
        }

        [Theory]
        [LoadData("artist")]
        public async Task should_add_new_item(Artist artist)
        {
            artist.ArtistId = Guid.NewGuid();

            var sut = new ArtistRepository(_factory.ContextInstance);
            sut.Add(artist);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _factory.ContextInstance.Artists
                .FirstOrDefault(x => x.ArtistId == artist.ArtistId)
                .ShouldNotBeNull();
        }
    }
}