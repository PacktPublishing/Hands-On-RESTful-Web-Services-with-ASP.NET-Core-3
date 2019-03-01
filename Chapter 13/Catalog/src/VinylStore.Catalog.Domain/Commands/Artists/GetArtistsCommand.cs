using System.Collections.Generic;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Artists
{
    public class GetArtistsCommand : IRequest<IList<ArtistResponse>>
    {
    }
}