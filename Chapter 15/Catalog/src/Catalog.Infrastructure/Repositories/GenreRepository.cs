using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CatalogContext _catalogContext;


        public GenreRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IUnitOfWork UnitOfWork => _catalogContext;

        public async Task<IEnumerable<Genre>> GetAsync()
        {
            return await _catalogContext.Genres
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Genre> GetAsync(Guid id)
        {
            var item = await _catalogContext.Genres
                .FindAsync(id);

            if (item == null) return null;

            _catalogContext.Entry(item).State = EntityState.Detached;
            return item;
        }

        public Genre Add(Genre item)
        {
            return _catalogContext.Genres.Add(item).Entity;
        }
    }
}