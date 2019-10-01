using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ArtistResponse>> GetArtistsAsync(CancellationToken cancellationToken)
        {
            var result = await _artistRepository.GetAsync();
            return _mapper.Map<IList<ArtistResponse>>(result);
        }

        public async Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _artistRepository.GetAsync(request.Id);
            return result == null ? null : new ArtistResponse { ArtistId = result.ArtistId, ArtistName = result.ArtistName };
        }

        public async Task<IList<ItemResponse>> GetItemByArtistIdAsync(GetItemsByArtistRequest request,
            CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByArtistIdAsync(request.Id);
            return _mapper.Map<List<ItemResponse>>(result);
        }

        public async Task<ArtistResponse> AddArtist(AddArtistRequest request, CancellationToken cancellationToken)
        {
            var item = new Entities.Artist { ArtistName = request.ArtistName };

            var result = _artistRepository.Add(item);
            await _artistRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new ArtistResponse { ArtistId = result.ArtistId, ArtistName = result.ArtistName };
        }
    }
}