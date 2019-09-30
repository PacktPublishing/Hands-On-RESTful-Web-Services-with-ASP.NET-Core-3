using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Infrastructure.Repositories
{
    public interface IItemRepository : IRepository
    {
        Task<IList<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);
        Item Delete(Item item);
    }
}