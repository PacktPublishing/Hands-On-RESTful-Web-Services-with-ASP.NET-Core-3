using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface IArtistRepository : IRepository
    {
        Task<IList<Artist>> GetAsync();
        Task<Artist> GetAsync(Guid id);
        Artist Add(Artist artist);
    }
}