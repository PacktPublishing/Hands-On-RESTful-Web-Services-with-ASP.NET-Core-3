using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Artist;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Artist
{
    public class AddArtistHandler : IRequestHandler<AddArtistCommand, ArtistResponse>
    {
        private readonly IArtistRepository _artistRepository;

        public AddArtistHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<ArtistResponse> Handle(AddArtistCommand command, CancellationToken cancellationToken)
        {
            var item = new Entities.Artist { ArtistName = command.ArtistName };

            var result = _artistRepository.Add(item);
            await _artistRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new ArtistResponse { ArtistId = result.ArtistId, ArtistName = result.ArtistName };
        }
    }
}