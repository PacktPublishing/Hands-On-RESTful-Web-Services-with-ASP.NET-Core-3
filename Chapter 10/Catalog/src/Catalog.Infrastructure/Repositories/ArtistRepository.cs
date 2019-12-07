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
        public IUnitOfWork UnitOfWork => _catalogContext;

        public ArtistRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Artist>> GetAsync()
        {
            return await _catalogContext.Artists
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Artist> GetAsync(Guid id)
        {
            var artist = await _catalogContext.Artists
                .FindAsync(id);

            if (artist == null) return null;

            _catalogContext.Entry(artist).State = EntityState.Detached;
            return artist;
        }

        public Artist Add(Artist artist)
        {
            return _catalogContext.Artists.Add(artist).Entity;
        }
    }
}