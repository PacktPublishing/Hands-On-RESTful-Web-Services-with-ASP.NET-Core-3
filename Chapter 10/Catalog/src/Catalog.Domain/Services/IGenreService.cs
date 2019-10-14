using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreResponse>> GetGenreAsync(CancellationToken cancellationToken);

        Task<GenreResponse> GetGenreAsync(GetGenreRequest request, CancellationToken cancellationToken);

        Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetItemsByGenreRequest request,
            CancellationToken cancellationToken);

        Task<GenreResponse> AddGenreAsync(AddGenreRequest request, CancellationToken cancellationToken);
    }
}