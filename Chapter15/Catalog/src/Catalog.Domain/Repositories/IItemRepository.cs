using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository : IRepository
    {
        Task<IEnumerable<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
        Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IEnumerable<Item>> GetItemByGenreIdAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);
    }
}