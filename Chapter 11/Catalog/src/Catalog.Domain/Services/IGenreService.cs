using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreResponse>> GetGenreAsync();
        Task<GenreResponse> GetGenreAsync(GetGenreRequest request);
        Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetGenreRequest request);
        Task<GenreResponse> AddGenreAsync(AddGenreRequest request);
    }
}