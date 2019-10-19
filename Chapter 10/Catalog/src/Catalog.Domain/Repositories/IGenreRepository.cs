using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IGenreRepository : IRepository
    {
        Task<IList<Genre>> GetAsync();
        Task<Genre> GetAsync(Guid id);
        Genre Add(Genre genre);
    }
}