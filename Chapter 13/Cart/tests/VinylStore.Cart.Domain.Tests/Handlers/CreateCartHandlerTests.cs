using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shouldly;
using VinylStore.Cart.Domain.Commands.Cart;
using VinylStore.Cart.Domain.Handlers.Cart;
using VinylStore.Cart.Domain.Infrastructure.Mapper;
using VinylStore.Cart.Fixtures;
using Xunit;

namespace VinylStore.Cart.Domain.Tests.Handlers
{
    public class CreateCartHandlerTests : IClassFixture<CartContextFactory>
    {
        public CreateCartHandlerTests(CartContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly CartContextFactory _contextFactory;

        [Fact]
        public async Task handle_should_create_a_new_record_and_return()
        {
            var handler = new CreateCartHandler(
                _contextFactory.GetCartRepository(),
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
                _contextFactory.GetCatalogService());

            var result = await handler.Handle(
                new CreateCartCommand
                {
                    ItemsIds = new[]
                        {"be05537d-5e80-45c1-bd8c-aa21c0f1251e", "f5da5ce4-091e-492e-a70a-22b073d75a52"},
                    UserEmail = "samuele.resca@gmail.com"
                }, CancellationToken.None);

            result.Id.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.ValidityDate.ShouldNotBeNull();
        }
    }
}
