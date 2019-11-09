using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository : IRepository
    {
        Task<IList<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
        Task<IList<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IList<Item>> GetItemByGenreIdAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);
    }
}