using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Artist
{
    public class GetArtistHandler : IRequestHandler<GetArtistCommand, ArtistResponse>
    {
        private readonly IArtistRepository _artistRepository;

        public GetArtistHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ArtistResponse> Handle(GetArtistCommand command, CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();
            var result = await _artistRepository.GetAsync(command.Id);

            return result == null
                ? null
                : new ArtistResponse { ArtistId = result.ArtistId, ArtistName = result.ArtistName };
        }
    }
}
