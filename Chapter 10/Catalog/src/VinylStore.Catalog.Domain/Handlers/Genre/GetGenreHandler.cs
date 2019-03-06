using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Genre
{
    public class GetGenreHandler : IRequestHandler<GetGenreCommand, GenreResponse>
    {
        private readonly IGenreRepository _genreRepository;

        public GetGenreHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreResponse> Handle(GetGenreCommand command, CancellationToken cancellationToken)
        {
            if (command?.Id == null) throw new ArgumentNullException();

            var result = await _genreRepository.GetAsync(command.Id);
            return result == null ? null : new GenreResponse { GenreId = result.GenreId, GenreDescription = result.GenreDescription };
        }
    }
}