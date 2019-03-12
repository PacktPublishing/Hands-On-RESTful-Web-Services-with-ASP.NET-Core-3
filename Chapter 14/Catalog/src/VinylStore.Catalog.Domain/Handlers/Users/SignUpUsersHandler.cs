using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.User;
using VinylStore.Catalog.Domain.Entities;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Users;

namespace VinylStore.Catalog.Domain.Handlers.Users
{
    public class SignUpUsersHandler : IRequestHandler<SignUpUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public SignUpUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<UserResponse> Handle(SignUpUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User { Email = command.Email, UserName = command.Email, Name = command.Name };
            bool result = await _userRepository.SignUp(user, command.Password, cancellationToken);

            return !result ? null : new UserResponse { Name = command.Name, Email = command.Email };
        }
    }
}
