using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Genre;
using VinylStore.Catalog.Domain.Infrastructure.Repositories;
using VinylStore.Catalog.Domain.Responses.Item;

namespace VinylStore.Catalog.Domain.Handlers.Genre
{
    public class AddGenreHandler : IRequestHandler<AddGenreCommand, GenreResponse>
    {
        private readonly IGenreRepository _genreRepository;

        public AddGenreHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreResponse> Handle(AddGenreCommand command, CancellationToken cancellationToken)
        {
            var item = new Entities.Genre {GenreDescription = command.GenreDescription};

            var result = _genreRepository.Add(item);
            await _genreRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new GenreResponse {GenreId = result.GenreId, GenreDescription = result.GenreDescription};
        }
    }
}