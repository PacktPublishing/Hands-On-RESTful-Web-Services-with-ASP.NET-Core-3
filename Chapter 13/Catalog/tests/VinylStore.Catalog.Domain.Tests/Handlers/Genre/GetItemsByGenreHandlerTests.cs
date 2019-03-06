using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Handlers.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Mapper;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Genre
{
    public class GetItemsByGenreHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        public GetItemsByGenreHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }


        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task handle_should_return_right_items_using_genre_id(string id)
        {
            var repository = new ItemRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetItemsByGenreHandler(repository,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result = await sut.Handle(new GetItemsByGenreCommand { Id = new Guid(id) }, CancellationToken.None);

            result.ShouldNotBeNull();
        }
    }
}