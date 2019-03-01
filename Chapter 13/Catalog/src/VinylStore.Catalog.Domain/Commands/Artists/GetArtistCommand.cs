using System;
using MediatR;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Commands.Artists
{
    public class GetArtistCommand : IRequest<ArtistResponse>
    {
        public Guid Id { get; set; }
    }
}