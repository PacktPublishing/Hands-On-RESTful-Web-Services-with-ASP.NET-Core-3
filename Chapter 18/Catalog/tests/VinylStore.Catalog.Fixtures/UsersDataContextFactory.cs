using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Moq;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;

namespace VinylStore.Catalog.Fixtures
{
    public class UsersDataContextFactory
    {
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IList<User> _users;

        public UsersDataContextFactory()
        {
            _passwordHasher = new PasswordHasher<User>();

            _users = new List<User>();

            var user = new User
            {
                Id = "test_id",
                Email = "samuele.resca@example.com",
                Name = "Samuele Resca"
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "P@$$w0rd");

            _users.Add(user);
        }

        public IUserRepository InMemoryUserManager => GetInMemoryUserManager();

        private IUserRepository GetInMemoryUserManager()
        {
            var fakeUserService = new Mock<IUserRepository>();

            fakeUserService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, string password, CancellationToken token) =>
                {
                    var user = _users.FirstOrDefault(x => x.Email == email);

                    if (user == null) return false;

                    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    return result == PasswordVerificationResult.Success;
                });

            fakeUserService.Setup(x => x.GetByEmail(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, CancellationToken token) => _users.First(x => x.Email == email));

            fakeUserService.Setup(x => x.SignUp(It.IsAny<User>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((User user, string password, CancellationToken token) =>
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    _users.Add(user);

                    return true;
                });

            return fakeUserService.Object;
        }
    }
}
