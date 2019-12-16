using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cart.Domain.Repositories
{
    public interface ICartRepository
    {
        IEnumerable<string> GetCarts();
        Task<Entities.CartSession> GetAsync(Guid id);
        Task<Entities.CartSession> AddOrUpdateAsync(Entities.CartSession item);
    }
}