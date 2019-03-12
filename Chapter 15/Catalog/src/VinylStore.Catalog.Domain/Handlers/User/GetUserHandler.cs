using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Users;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Users;

namespace VinylStore.Catalog.Domain.Handlers.User
{
    public class GetUserHandler : IRequestHandler<GetUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(GetUserCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _userRepository.GetByEmail(command.Email, cancellationToken);
            return new UserResponse { Name = response.Name, Email = response.Email };
        }
    }
}
