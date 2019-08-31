using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Shouldly;
using VinylStore.Catalog.Domain.Commands.User;
using VinylStore.Catalog.Domain.Handlers.Users;
using VinylStore.Catalog.Fixtures;
using Xunit;

namespace VinylStore.Catalog.Domain.Tests.Handlers.User
{
    public class SignUpUsersHandlerTests : IClassFixture<UsersDataContextFactory>
    {
        public SignUpUsersHandlerTests(UsersDataContextFactory testDataContextFactory)
        {
            _testDataContextFactory = testDataContextFactory;
        }

        private readonly UsersDataContextFactory _testDataContextFactory;


        [Fact]
        public async Task signup_should_create_a_new_user()
        {
            var sut = new SignUpUsersHandler(_testDataContextFactory.InMemoryUserManager);
            var newEmail = "samuele.resca.newaccount@example.com";
            var name = "Samuele Resca";


            var result =
                await sut.Handle(
                    new SignUpUserCommand
                    { Name = name, Email = newEmail, Password = "P@$$w0rd" },
                    CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(name);
            result.Email.ShouldBe(newEmail);
        }
    }
}