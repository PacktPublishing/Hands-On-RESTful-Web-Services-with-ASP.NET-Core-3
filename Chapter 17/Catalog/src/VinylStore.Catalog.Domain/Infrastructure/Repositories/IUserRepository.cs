using System.Threading;
using System.Threading.Tasks;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Domain.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(string email, string password,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SignUp(User user, string password, CancellationToken cancellationToken = default(CancellationToken));
        Task<User> GetByEmail(string requestEmail, CancellationToken cancellationToken = default(CancellationToken));
    }
}
