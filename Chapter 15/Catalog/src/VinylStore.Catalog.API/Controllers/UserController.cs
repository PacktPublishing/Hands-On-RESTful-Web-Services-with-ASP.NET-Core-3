using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylStore.Catalog.API.Infrastructure.Filters;
using VinylStore.Catalog.Domain.Commands.Users;

namespace VinylStore.Catalog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    [JsonException]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (claim == null) return Unauthorized();

            var token = await _mediator.Send(new GetUserCommand { Email = claim.Value });
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> SignIn(SignInUserCommand request)
        {
            var token = await _mediator.Send(request);

            if (token == null) return BadRequest();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserCommand request)
        {
            var user = await _mediator.Send(request);

            if (user == null) return BadRequest();

            return CreatedAtAction(nameof(Get), new { }, null);
        }
    }
}
