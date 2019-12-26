using System.Threading.Tasks;
using Catalog.Domain.Requests.User;
using Catalog.Domain.Services;
using Catalog.Domain.Settings;
using Catalog.Fixtures;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests.Services
{
    public class UserServiceTests : IClassFixture<UsersContextFactory>
    {
        private readonly IUserService _userService;

        public UserServiceTests(UsersContextFactory usersContextFactory)
        {
            _userService = new UserService(usersContextFactory.InMemoryUserManager,
                Options.Create(
                    new AuthenticationSettings { Secret = "Very Secret key-word to match", ExpirationDays = 7 }));
        }

        [Fact]
        public async Task signin_with_invalid_user_should_return_a_valid_token_response()
        {
            var result =
                await _userService.SignInAsync(new SignInRequest { Email = "invalid.user", Password = "invalid_password" });
            result.ShouldBeNull();
        }

        [Fact]
        public async Task signin_with_valid_user_should_return_a_valid_token_response()
        {
            var result =
                await _userService.SignInAsync(new SignInRequest { Email = "samuele.resca@example.com", Password = "P@$$w0rd" });
            result.Token.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task signup_should_create_a_new_user()
        {
            var newEmail = "samuele.resca.newaccount@example.com";
            var name = "Samuele Resca";

            var result =
                await _userService.SignUpAsync(new SignUpRequest
                { Name = name, Email = newEmail, Password = "P@$$w0rd" });

            result.Name.ShouldBe(name);
            result.Email.ShouldBe(newEmail);
        }
    }
}