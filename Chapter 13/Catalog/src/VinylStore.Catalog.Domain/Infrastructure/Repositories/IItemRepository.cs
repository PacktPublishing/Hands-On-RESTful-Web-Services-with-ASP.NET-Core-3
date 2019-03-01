using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Domain.Infrastructure.Repositories
{
    public interface IItemRepository : IRepository
    {
        Task<IList<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
        Task<IList<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IList<Item>> GetItemByGenreIdAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);
        Item Delete(Item item);
    }
}