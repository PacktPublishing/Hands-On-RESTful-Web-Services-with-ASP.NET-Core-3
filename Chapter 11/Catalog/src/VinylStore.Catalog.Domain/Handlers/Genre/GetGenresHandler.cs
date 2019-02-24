using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Genre
{
    public class GetGenresHandler : IRequestHandler<GetGenresCommand, List<GenreResponse>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetGenresHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<List<GenreResponse>> Handle(GetGenresCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _genreRepository.GetAsync();

            return _mapper.Map<List<GenreResponse>>(result);
        }
    }
}