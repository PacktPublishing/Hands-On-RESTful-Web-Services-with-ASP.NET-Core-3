using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.User;
using VinylStore.Catalog.Domain.Handlers.User;
using VinylStore.Catalog.Domain.Infrastructure.Settings;
using VinylStore.Catalog.Fixtures;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.User
{
    public class SignInUsersHandlerTests : IClassFixture<UsersDataContextFactory>
    {
        public SignInUsersHandlerTests(UsersDataContextFactory testDataContextFactory)
        {
            _testDataContextFactory = testDataContextFactory;
        }

        private readonly UsersDataContextFactory _testDataContextFactory;

        [Fact]
        public async Task signin_with_invalid_user_should_return_a_valid_token_response()
        {
            var sut = new SignInUserHandler(_testDataContextFactory.InMemoryUserManager,
                Options.Create(
                    new AuthenticationSettings { Secret = "Very Secret key-word to match", ExpirationDays = 7 }));

            var result =
                await sut.Handle(new SignInUserCommand { Email = "invalid.user", Password = "invalid_password" },
                    CancellationToken.None);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task signin_with_valid_user_should_return_a_valid_token_response()
        {
            var sut = new SignInUserHandler(_testDataContextFactory.InMemoryUserManager,
                Options.Create(
                    new AuthenticationSettings { Secret = "Very Secret key-word to match", ExpirationDays = 7 }));

            var result =
                await sut.Handle(new SignInUserCommand { Email = "samuele.resca@example.com", Password = "P@$$w0rd" },
                    CancellationToken.None);

            result.ShouldNotBeNull();
            result.Token.ShouldNotBeEmpty();
        }
    }
}
