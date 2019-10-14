using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Mapper;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Services
{
    public class ItemServiceTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public ItemServiceTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        [Fact]
        public async Task getitems_should_return_right_data()
        {
            var sut = new ItemService(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.GetItems(CancellationToken.None);

            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getitem_should_return_right_data(string guid)
        {
            var sut = new ItemService(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.GetItem(new GetItemRequest { Id = new Guid(guid) }, CancellationToken.None);

            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public void getitem_should_thrown_exception_with_null_id()
        {
            var sut = new ItemService(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            sut.GetItem(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [InlineData(
            "{\"Name\":\"Test album\",\"Description\":\"Description\",\"LabelName\":\"LabelName\",\"Price\":{\"Amount\":23.5,\"Currency\":\"EUR\"},\"PictureUri\":\"https://mycdn.com/pictures/32423423\",\"ReleaseDate\":\"2016-01-01T00:00:00+00:00\",\"Format\":\"Vinyl 33g\",\"AvailableStock\":6,\"GenreId\":\"c04f05c0-f6ad-44d1-a400-3375bfb5dfd6\",\"Genre\":null,\"ArtistId\":\"f08a333d-30db-4dd1-b8ba-3b0473c7cdab\",\"Artist\":null}")]
        public async Task additem_should_add_right_entity(string json)
        {
            var sut = new ItemService(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var item = JsonConvert.DeserializeObject<AddItemRequest>(json);

            var result =
                await sut.AddItem(item, CancellationToken.None);

            result.Name.ShouldBe(item.Name);
            result.Description.ShouldBe(item.Description);
            result.GenreId.ShouldBe(item.GenreId);
            result.ArtistId.ShouldBe(item.ArtistId);
            result.Price.Amount.ShouldBe(item.Price.Amount);
            result.Price.Currency.ShouldBe(item.Price.Currency);
        }

        [Theory]
        [InlineData(
            "{\"Id\":\"b5b05534-9263-448c-a69e-0bbd8b3eb90e\", \"Name\":\"Test album\",\"Description\":\"Description\",\"LabelName\":\"LabelName\",\"Price\":{\"Amount\":23.5,\"Currency\":\"EUR\"},\"PictureUri\":\"https://mycdn.com/pictures/32423423\",\"ReleaseDate\":\"2016-01-01T00:00:00+00:00\",\"Format\":\"Vinyl 33g\",\"AvailableStock\":6,\"GenreId\":\"c04f05c0-f6ad-44d1-a400-3375bfb5dfd6\",\"Genre\":null,\"ArtistId\":\"f08a333d-30db-4dd1-b8ba-3b0473c7cdab\",\"Artist\":null}")]
        public async Task edititem_should_add_right_entity(string json)
        {
            var sut = new ItemService(new ItemRepository(_catalogDataContextFactory.ContextInstance),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var item = JsonConvert.DeserializeObject<EditItemRequest>(json);

            var result =
                await sut.EditItem(item, CancellationToken.None);

            result.Name.ShouldBe(item.Name);
            result.Description.ShouldBe(item.Description);
            result.GenreId.ShouldBe(item.GenreId);
            result.ArtistId.ShouldBe(item.ArtistId);
            result.Price.Amount.ShouldBe(item.Price.Amount);
            result.Price.Currency.ShouldBe(item.Price.Currency);
        }
    }
}