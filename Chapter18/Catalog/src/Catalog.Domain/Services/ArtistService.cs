using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses;

namespace Catalog.Domain.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistMapper _artistMapper;
        private readonly IArtistRepository _artistRepository;
        private readonly IItemMapper _itemMapper;
        private readonly IItemRepository _itemRepository;

        public ArtistService(
            IArtistRepository artistRepository,
            IItemRepository itemRepository,
            IArtistMapper artistMapper,
            IItemMapper itemMapper)
        {
            _artistRepository = artistRepository;
            _itemRepository = itemRepository;
            _artistMapper = artistMapper;
            _itemMapper = itemMapper;
        }

        public async Task<IEnumerable<ArtistResponse>> GetArtistsAsync()
        {
            var result = await _artistRepository.GetAsync();
            return result.Select(_artistMapper.Map);
        }

        public async Task<ArtistResponse> GetArtistAsync(GetArtistRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _artistRepository.GetAsync(request.Id);
            return result == null ? null : _artistMapper.Map(result);
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByArtistIdAsync(request.Id);
            return result.Select(_itemMapper.Map);
        }

        public async Task<ArtistResponse> AddArtistAsync(AddArtistRequest request)
        {
            var item = new Artist { ArtistName = request.ArtistName };
            var result = _artistRepository.Add(item);

            await _artistRepository.UnitOfWork.SaveChangesAsync();

            return _artistMapper.Map(result);
        }
    }
}