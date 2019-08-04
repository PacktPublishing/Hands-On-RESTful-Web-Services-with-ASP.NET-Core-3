using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VinylStore.Cart.Domain.Infrastructure.Repositories
{
    public interface ICartRepository
    {
        IEnumerable<string> GetCarts();
        Task<Entities.Cart> GetAsync(Guid id);
        Task<Entities.Cart> AddOrUpdateAsync(Entities.Cart item);
    }
}
