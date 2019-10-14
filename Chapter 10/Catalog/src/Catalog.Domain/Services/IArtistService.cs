using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistResponse>> GetArtistsAsync(CancellationToken cancellationToken);

        Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken cancellationToken);

        Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetItemsByArtistRequest request,
            CancellationToken cancellationToken);

        Task<ArtistResponse> AddArtist(AddArtistRequest request, CancellationToken cancellationToken);
    }
}