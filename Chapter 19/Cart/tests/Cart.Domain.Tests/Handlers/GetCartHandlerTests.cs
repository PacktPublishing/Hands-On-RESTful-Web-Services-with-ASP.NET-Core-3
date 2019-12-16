using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cart.Domain.Commands.Cart;
using Cart.Domain.Handlers.Cart;
using Cart.Domain.Mapper;
using Cart.Fixtures;
using Shouldly;
using Xunit;

namespace Cart.Domain.Tests.Handlers
{
    public class GetCartHandlerTests : IClassFixture<CartContextFactory>
    {
        public GetCartHandlerTests(CartContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly CartContextFactory _contextFactory;

        [Fact]
        public async Task handle_should_retrieve_a_new_record_and_return_it()
        {
            var handler = new GetCartHandler(
                _contextFactory.GetCartRepository(),
                new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
                _contextFactory.GetCatalogService());
            var result = await handler.Handle(
                new GetCartCommand
                {
                    Id = new Guid("9ced6bfa-9659-462e-aece-49fe50613e96")
                }, CancellationToken.None);

            result.Id.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.ValidityDate.ShouldNotBeNull();
        }
    }
}