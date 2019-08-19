using System.Collections.Generic;
using MediatR;
using VinylStore.Cart.Domain.Responses.Cart;

namespace VinylStore.Cart.Domain.Commands.Cart
{
    public class CreateCartCommand : IRequest<CartExtendedResponse>
    {
        public List<string> ItemsIds { get; set; }

        public string UserEmail { get; set; }
    }
}
