using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Artist
{
    public class AddArtistCommand : IRequest<ArtistResponse>
    {
        public string ArtistName { get; set; }
    }
}