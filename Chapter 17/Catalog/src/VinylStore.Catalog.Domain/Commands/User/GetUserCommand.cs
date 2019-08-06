using MediatR;
using VinylStore.Catalog.Domain.Responses.Users;

namespace VinylStore.Catalog.Domain.Commands.User
{
    public class GetUserCommand : IRequest<UserResponse>
    {
        public string Email { get; set; }
    }
}
