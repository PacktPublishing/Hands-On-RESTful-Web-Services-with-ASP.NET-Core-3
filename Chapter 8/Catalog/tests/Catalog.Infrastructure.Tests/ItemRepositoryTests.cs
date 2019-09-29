using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Infrastructure.Tests
{
    public class ItemRepositoryTests
    {
        [Fact]
        public async Task should_get_data()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_get_data")
                .Options;

            using (var context = new TestCatalogContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestCatalogContext(options))
            {
                var sut = new ItemRepository(context);
                var result = await sut.GetAsync();
                result
                    .Count
                    .ShouldBe(4);
            }
        }

        [Fact]
        public async Task should_returns_null_with_id_not_present()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_returns_null_with_id_not_present")
                .Options;

            using (var context = new TestCatalogContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestCatalogContext(options))
            {
                var sut = new ItemRepository(context);
                var result = await sut.GetAsync(Guid.NewGuid());
                result.ShouldBeNull();
            }
        }


        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task should_return_record_by_id(string guid)
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_return_record_by_id")
                .Options;

            using (var context = new TestCatalogContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestCatalogContext(options))
            {
                var sut = new ItemRepository(context);
                var result = await sut.GetAsync(new Guid(guid));
                result.Id.ShouldBe(new Guid(guid));
            }
        }

        [Theory]
        [InlineData("{ \"Id\": \"b5b05534-9263-448c-a56e-0bbd8b3eb90e\", \"Name\": \"Test album\", \"Description\": \"Description\", \"LabelName\": \"LabelName\", \"Price\": { \"Amount\": 23.5, \"Currency\": \"EUR\" }, \"PictureUri\": \"https://mycdn.com/pictures/32423423\", \"ReleaseDate\": \"2016-01-01T00:00:00+00:00\", \"Format\": \"Vinyl 33g\", \"AvailableStock\": 6, \"GenreId\": \"c04f05c0-f6ad-44d1-a400-3375bfb5dfd6\", \"Genre\": null, \"ArtistId\": \"f08a333d-30db-4dd1-b8ba-3b0473c7cdab\", \"Artist\": null }")]
        public async Task should_add_new_item(string jsonEntity)
        {
            var entity = JsonConvert.DeserializeObject<Item>(jsonEntity);

            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_add_new_items")
                .Options;

            using (var context = new TestCatalogContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestCatalogContext(options))
            {
                var sut = new ItemRepository(context);
                sut.Add(entity);

                await sut.UnitOfWork.SaveEntitiesAsync();
            }

            using (var context = new TestCatalogContext(options))
            {
                context.Items
                    .FirstOrDefault(_ => _.Id == entity.Id)
                    .ShouldNotBeNull();
            }

        }

        [Theory]
        [InlineData("{ \"Id\": \"b5b05534-9263-448c-a69e-0bbd8b3eb90e\", \"Name\": \"GOOD KID, m.A.A.d CITY\", \"Description\": \"GOOD KID, m.A.A.d CITY. by Kendrick Lamar\", \"LabelName\": \"TDE, Top Dawg Entertainment\", \"Price\": { \"Amount\": 23.5, \"Currency\": \"EUR\" }, \"PictureUri\": \"https://mycdn.com/pictures/32423423\", \"ReleaseDate\": \"2016-01-01T00:00:00+00:00\", \"Format\": \"Vinyl 33g\", \"AvailableStock\": 6, \"GenreId\": \"c04f05c0-f6ad-44d1-a400-3375bfb5dfd6\", \"Genre\": null, \"ArtistId\": \"f08a333d-30db-4dd1-b8ba-3b0473c7cdab\", \"Artist\": null }")]
        public async Task should_update_item(string jsonEntity)
        {
            var entity = JsonConvert.DeserializeObject<Item>(jsonEntity);
            entity.Description = "Updated";

            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_update_item")
                .Options;

            using (var context = new TestCatalogContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new TestCatalogContext(options))
            {
                var sut = new ItemRepository(context);

                sut.Update(entity);
                await sut.UnitOfWork.SaveEntitiesAsync();
            }

            using (var context = new TestCatalogContext(options))
            {
                context.Items
                    .FirstOrDefault(_ => _.Id == entity.Id)
                    ?.Description.ShouldBe("Updated");
            }
        }
    }
}