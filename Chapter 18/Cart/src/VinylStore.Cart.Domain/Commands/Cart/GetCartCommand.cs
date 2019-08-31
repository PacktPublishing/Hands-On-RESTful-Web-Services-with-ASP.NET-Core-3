using System;
using MediatR;
using VinylStore.Cart.Domain.Responses.Cart;

namespace VinylStore.Cart.Domain.Commands.Cart
{
    public class GetCartCommand : IRequest<CartExtendedResponse>
    {
        public Guid Id { get; set; }
    }
}
