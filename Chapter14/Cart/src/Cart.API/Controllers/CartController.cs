using System;
using System.Threading.Tasks;
using Cart.API.Infrastructure.Filters;
using Cart.Domain.Commands.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [JsonException]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCartCommand { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCartCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [HttpPut("{cartId:guid}/items/{id:guid}")]
        public async Task<IActionResult> Put(Guid cartId, Guid id)
        {
            var result = await _mediator.Send(new UpdateCartItemQuantityCommand
            {
                CartId = cartId,
                CartItemId = id,
                IsAddOperation = true
            });

            return Ok(result);
        }

        [HttpDelete("{cartId:guid}/items/{id:guid}")]
        public async Task<IActionResult> Delete(Guid cartId, Guid id)
        {
            var result = await _mediator.Send(new UpdateCartItemQuantityCommand
            {
                CartId = cartId,
                CartItemId = id,
                IsAddOperation = false
            });

            return Ok(result);
        }
    }
}