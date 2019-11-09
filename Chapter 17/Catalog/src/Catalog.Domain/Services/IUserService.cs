using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.User;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken cancellationToken = default);
        Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    }
}