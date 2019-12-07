using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ItemRepository
        : IItemRepository
    {
        private readonly CatalogContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ItemRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Item>> GetAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null) return null;

            await _context.Entry(item)
                .Reference(i => i.Artist).LoadAsync();
            await _context.Entry(item)
                .Reference(i => i.Genre).LoadAsync();

            return item;
        }

        public Item Add(Item item)
        {
            return _context.Items.Add(item).Entity;
        }

        public Item Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public Item Delete(Item item)
        {
            throw new NotImplementedException();
        }
    }
}