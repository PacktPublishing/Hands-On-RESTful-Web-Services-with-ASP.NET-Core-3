using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly CatalogContext _catalogContext;

        public ArtistRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IUnitOfWork UnitOfWork => _catalogContext;

        public async Task<IList<Artist>> GetAsync()
        {
            return await _catalogContext.Artists
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Artist> GetAsync(Guid id)
        {
            var item = await _catalogContext.Artists
                .FindAsync(id);

            if (item == null) return null;

            _catalogContext.Entry(item).State = EntityState.Detached;
            return item;
        }

        public Artist Add(Artist item)
        {
            return _catalogContext.Artists.Add(item).Entity;
        }
    }
}