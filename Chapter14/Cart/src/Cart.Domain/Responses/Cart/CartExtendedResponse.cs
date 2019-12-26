using System;
using System.Collections.Generic;
using Cart.Domain.Entities;

namespace Cart.Domain.Responses.Cart
{
    public class CartExtendedResponse
    {
        public string Id { get; set; }

        public IList<CartItemResponse> Items { get; set; }

        public CartUser User { get; set; }

        public DateTimeOffset ValidityDate { get; set; }
    }
}