using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Domain.Infrastructure.Repositories
{
    public interface IGenreRepository : IRepository
    {
        Task<IList<Genre>> GetAsync();
        Task<Genre> GetAsync(Guid id);
        Genre Add(Genre item);
    }
}
