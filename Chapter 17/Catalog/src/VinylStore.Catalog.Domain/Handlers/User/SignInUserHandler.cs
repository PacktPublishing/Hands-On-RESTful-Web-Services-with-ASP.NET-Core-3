using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VinylStore.Catalog.Domain.Commands.User;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Infrastructure.Settings;
using VinylStore.Catalog.Domain.Responses.Users;

namespace VinylStore.Catalog.Domain.Handlers.User
{
    public class SignInUserHandler : IRequestHandler<SignInUserCommand, TokenResponse>
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserRepository _userRepository;

        public SignInUserHandler(IUserRepository userRepository,
            IOptions<AuthenticationSettings> authenticationSettings)
        {
            _userRepository = userRepository;
            _authenticationSettings = authenticationSettings.Value;
        }

        public async Task<TokenResponse> Handle(SignInUserCommand command,
            CancellationToken cancellationToken)
        {
            bool response = await _userRepository.Authenticate(command.Email, command.Password, cancellationToken);

            return response == false ? null : new TokenResponse { Token = GenerateSecurityToken(command) };
        }

        private string GenerateSecurityToken(SignInUserCommand command)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, command.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.ExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
