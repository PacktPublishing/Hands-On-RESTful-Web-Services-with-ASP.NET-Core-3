using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Catalog.Infrastructure.Tests
{
    public class ItemRepositoryTests : IClassFixture<CatalogDataContextFactory>
    {
        public ItemRepositoryTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        [Fact]
        public async Task should_get_data()
        {
            var sut = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var result = await sut.GetAsync();

            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task should_returns_null_with_id_not_present()
        {
            var sut = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var result = await sut.GetAsync(Guid.NewGuid());

            result.ShouldBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task should_return_record_by_id(string guid)
        {
            var sut = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var result = await sut.GetAsync(new Guid(guid));

            result.Id.ShouldBe(new Guid(guid));
        }

        [Theory]
        [LoadTestData("record-test.json", "item_without_id")]
        public async Task should_add_new_item(Item entity)
        {
            var sut = new ItemRepository(_catalogDataContextFactory.ContextInstance);
            sut.Add(entity);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _catalogDataContextFactory.ContextInstance.Items
                .FirstOrDefault(item => item.Id == entity.Id)
                .ShouldNotBeNull();
        }

        [Theory]
        [LoadTestData("record-test.json", "item_with_id")]
        public async Task should_update_an_item(Item entity)
        {
            entity.Description = "Updated";

            var sut = new ItemRepository(_catalogDataContextFactory.ContextInstance);
            var result = sut.Update(entity);

            await sut.UnitOfWork.SaveEntitiesAsync();

            _catalogDataContextFactory.ContextInstance.Items
                .FirstOrDefault(item => item.Id == result.Id)
                ?.Description.ShouldBe("Updated");
        }
    }
}