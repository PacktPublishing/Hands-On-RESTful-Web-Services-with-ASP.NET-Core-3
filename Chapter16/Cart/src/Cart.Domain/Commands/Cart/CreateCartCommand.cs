using Cart.Domain.Responses.Cart;
using MediatR;

namespace Cart.Domain.Commands.Cart
{
    public class CreateCartCommand : IRequest<CartExtendedResponse>
    {
        public string[] ItemsIds { get; set; }

        public string UserEmail { get; set; }
    }
}