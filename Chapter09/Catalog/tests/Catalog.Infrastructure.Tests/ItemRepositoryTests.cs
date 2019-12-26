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
    public class ItemRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _sut;
        private readonly TestCatalogContext _context;

        public ItemRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _context = catalogContextFactory.ContextInstance;
            _sut = new ItemRepository(_context);
        }

        [Fact]
        public async Task should_get_data()
        {
            var result = await _sut.GetAsync();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task should_returns_null_with_id_not_present()
        {
            var result = await _sut.GetAsync(Guid.NewGuid());
            result.ShouldBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task should_return_record_by_id(string guid)
        {
            var result = await _sut.GetAsync(new Guid(guid));
            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public async Task should_add_new_item()
        {
            var testItem = new Item
            {
                Name = "Test album",
                Description = "Description",
                LabelName = "Label name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                Format = "Vinyl 33g",
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };


            _sut.Add(testItem);
            await _sut.UnitOfWork.SaveEntitiesAsync();

            _context.Items
                .FirstOrDefault(item => item.Id == testItem.Id)
                .ShouldNotBeNull();
        }

        [Fact]
        public async Task should_update_item()
        {
            var testItem = new Item
            {
                Id = new Guid("f5da5ce4-091e-492e-a70a-22b073d75a52"),
                Name = "Test album",
                Description = "Description updated",
                LabelName = "Label name",
                Price = new Price { Amount = 50, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                Format = "Vinyl 33g",
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };


            _sut.Update(testItem);

            await _sut.UnitOfWork.SaveEntitiesAsync();

            var result = _context.Items
                .FirstOrDefault(item => item.Id == testItem.Id);

            result.Description.ShouldBe("Description updated");
            result.Price.Amount.ShouldBe(50);
        }
    }
}