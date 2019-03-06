using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Handlers.Genre;
using VinylStore.Catalog.Fixtures;
using VinylStore.Catalog.Infrastructure.Repositories;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.Genre
{
    public class GetGenreHandlerTests : IClassFixture<CatalogDataContextFactory>
    {
        public GetGenreHandlerTests(CatalogDataContextFactory catalogDataContextFactory)
        {
            _catalogDataContextFactory = catalogDataContextFactory;
        }

        private readonly CatalogDataContextFactory _catalogDataContextFactory;


        [Theory]
        [InlineData("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6")]
        public async Task getgenre_should_return_right_genre(string id)
        {
            var repository = new GenreRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetGenreHandler(repository);

            var result = await sut.Handle(new GetGenreCommand { Id = new Guid(id) }, CancellationToken.None);

            result.ShouldNotBeNull();
        }

        [Fact]
        public void getgenre_should_thrown_exception_with_null_id()
        {
            var repository = new GenreRepository(_catalogDataContextFactory.ContextInstance);

            var sut = new GetGenreHandler(repository);

            sut.Handle(null, CancellationToken.None).ShouldThrow<ArgumentNullException>();
        }
    }
}