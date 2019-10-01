using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistResponse>> GetArtistsAsync(CancellationToken cancellationToken);

        Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken cancellationToken);

        Task<IList<ItemResponse>> GetItemByArtistIdAsync(GetItemsByArtistRequest request,
            CancellationToken cancellationToken);

        Task<ArtistResponse> AddArtist(AddArtistRequest request, CancellationToken cancellationToken);
    }
}