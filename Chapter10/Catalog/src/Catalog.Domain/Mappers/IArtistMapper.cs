using Catalog.Domain.Entities;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Mappers
{
    public interface IArtistMapper
    {
        ArtistResponse Map(Artist artist);
    }
}