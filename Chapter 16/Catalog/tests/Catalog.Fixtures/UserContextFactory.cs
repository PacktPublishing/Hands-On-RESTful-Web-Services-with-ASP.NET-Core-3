using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Catalog.Fixtures
{
    public class UsersContextFactory
    {
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<User> _users;

        public UsersContextFactory()
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

            fakeUserService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, string password, CancellationToken token) =>
                {
                    var user = _users.FirstOrDefault(x => x.Email == email);

                    if (user == null) return false;

                    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    return result == PasswordVerificationResult.Success;
                });

            fakeUserService.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, CancellationToken token) => _users.First(x => x.Email == email));

            fakeUserService.Setup(x => x.SignUpAsync(It.IsAny<User>(), It.IsAny<string>(), CancellationToken.None))
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