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
    public class GetGenresHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        public GetGenresHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;

        [Fact]
        public async Task getgenre_should_return_right_data()
        {
            var repository = new GenreRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetGenresHandler(repository,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CatalogProfile>())));

            var result =
                await sut.Handle(new GetGenresCommand(), CancellationToken.None);

            result.Count.ShouldBe(1);
        }
    }
}