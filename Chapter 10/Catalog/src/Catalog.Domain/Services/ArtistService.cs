using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IArtistMapper _artistMapper;
        private readonly IItemMapper _itemMapper;

        public ArtistService(IArtistRepository artistRepository, IItemRepository itemRepository, 
            IArtistMapper artistMapper, IItemMapper itemMapper)
        {
            _artistRepository = artistRepository;
            _itemRepository = itemRepository;
            _artistMapper = artistMapper;
            _itemMapper = itemMapper;
        }


        public async Task<IEnumerable<ArtistResponse>> GetArtistsAsync(CancellationToken cancellationToken)
        {
            var result = await _artistRepository.GetAsync();
            return result.Select(_artistMapper.Map);
        }

        public async Task<ArtistResponse> GetArtistAsync(GetArtistRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _artistRepository.GetAsync(request.Id);
            return result == null ? null : new ArtistResponse { ArtistId = result.ArtistId, ArtistName = result.ArtistName };
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetItemsByArtistRequest request,
            CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByArtistIdAsync(request.Id);
            return result.Select(_itemMapper.Map);
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