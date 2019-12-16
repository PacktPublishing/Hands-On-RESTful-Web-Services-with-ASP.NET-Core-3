using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cart.Domain.Entities;

namespace Cart.Domain.Repositories
{
    public interface ICartRepository
    {
        IEnumerable<string> GetCarts();
        Task<CartSession> GetAsync(Guid id);
        Task<CartSession> AddOrUpdateAsync(CartSession item);
    }
}