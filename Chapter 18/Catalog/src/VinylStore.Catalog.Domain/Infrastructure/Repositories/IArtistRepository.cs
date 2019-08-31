using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VinylStore.Catalog.Domain.Entities;

namespace VinylStore.Catalog.Domain.Infrastructure.Repositories
{
    public interface IArtistRepository : IRepository
    {
        Task<IList<Artist>> GetAsync();
        Task<Artist> GetAsync(Guid id);
        Artist Add(Artist item);
    }
}
