using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.User;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Users;

namespace VinylStore.Catalog.Domain.Handlers.User
{
    public class SignUpUserHandler : IRequestHandler<SignUpUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public SignUpUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<UserResponse> Handle(SignUpUserCommand command, CancellationToken cancellationToken)
        {
            var user = new Entities.User { Email = command.Email, UserName = command.Email, Name = command.Name };
            bool result = await _userRepository.SignUp(user, command.Password, cancellationToken);

            return !result ? null : new UserResponse { Name = command.Name, Email = command.Email };
        }
    }
}
