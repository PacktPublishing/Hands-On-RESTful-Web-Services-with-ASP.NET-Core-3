using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Artist
{
    public class GetArtistsHandler : IRequestHandler<GetArtistsCommand, IList<ArtistResponse>>
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public GetArtistsHandler(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<IList<ArtistResponse>> Handle(GetArtistsCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _artistRepository.GetAsync();
            return _mapper.Map<IList<ArtistResponse>>(result);
        }
    }
}
