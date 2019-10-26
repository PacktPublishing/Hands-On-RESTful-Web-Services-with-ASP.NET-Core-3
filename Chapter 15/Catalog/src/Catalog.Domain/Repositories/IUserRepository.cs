using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Authenticate(string email, string password, CancellationToken cancellationToken = default);
        Task<bool> SignUp(User user, string password, CancellationToken cancellationToken = default);
        Task<User> GetByEmail(string requestEmail, CancellationToken cancellationToken = default);
    }
}