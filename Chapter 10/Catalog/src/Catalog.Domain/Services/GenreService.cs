using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository genreRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<GenreResponse>> GetGenreAsync(CancellationToken cancellationToken)
        {
            var result = await _genreRepository.GetAsync();
            return _mapper.Map<IEnumerable<GenreResponse>>(result);
        }

        public async Task<GenreResponse> GetGenreAsync(GetGenreRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _genreRepository.GetAsync(request.Id);
            return result == null ? null : new GenreResponse { GenreId = result.GenreId, GenreDescription = result.GenreDescription };
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetItemsByGenreRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var result = await _itemRepository.GetItemByGenreIdAsync(request.Id);
            return _mapper.Map<List<ItemResponse>>(result);
        }

        public async Task<GenreResponse> AddGenreAsync(AddGenreRequest request, CancellationToken cancellationToken)
        {
            var item = new Entities.Genre { GenreDescription = request.GenreDescription };

            var result = _genreRepository.Add(item);
            await _genreRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new GenreResponse { GenreId = result.GenreId, GenreDescription = result.GenreDescription };
        }
    }
}