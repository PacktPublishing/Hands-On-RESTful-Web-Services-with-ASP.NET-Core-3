using System;
using Cart.Domain.Responses.Cart;
using MediatR;

namespace Cart.Domain.Commands.Cart
{
    public class GetCartCommand : IRequest<CartExtendedResponse>
    {
        public Guid Id { get; set; }
    }
}