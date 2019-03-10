using System;
using MediatR;
using VinylStore.Cart.Domain.Responses.Cart;

namespace VinylStore.Cart.Domain.Commands.Cart
{
    public class UpdateCartItemQuantityCommand : IRequest<CartExtendedResponse>
    {
        public Guid CartId { get; set; }

        public Guid CartItemId { get; set; }

        public bool IsAddOperation { get; set; }
    }
}
