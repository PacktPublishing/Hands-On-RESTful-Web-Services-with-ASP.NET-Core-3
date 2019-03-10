using System;
using System.Collections.Generic;
using VinylStore.Cart.Domain.Entities;

namespace VinylStore.Cart.Domain.Responses.Cart
{
    public class CartExtendedResponse
    {
        public string Id { get; set; }

        public IList<CartItemResponse> Items { get; set; }

        public CartUser User { get; set; }

        public DateTimeOffset ValidityDate { get; set; }
    }
}
