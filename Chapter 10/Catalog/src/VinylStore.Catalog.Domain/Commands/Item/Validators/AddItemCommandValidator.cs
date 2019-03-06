using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using VinylStore.Catalog.Domain.Commands.Artists;
using VinylStore.Catalog.Domain.Commands.Genre;

namespace VinylStore.Catalog.Domain.Commands.Item.Validators
{
    public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
    {
        private readonly IMediator _mediator;

        public AddItemCommandValidator(IMediator mediator)
        {
            _mediator = mediator;

            RuleFor(x => x.ArtistId)
                .NotEmpty()
                .MustAsync(ArtistExists).WithMessage("Artist must exists");
            RuleFor(x => x.GenreId)
                .NotEmpty()
                .MustAsync(GenreExists).WithMessage("Genre must exists");
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.ReleaseDate).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.AvailableStock).Must(x => x > 0);
        }

        private async Task<bool> ArtistExists(Guid artistId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(artistId.ToString()))
            {
                return false;
            }

            var artist = await _mediator.Send(new GetArtistCommand { Id = artistId }, token);
            return artist != null;
        }

        private async Task<bool> GenreExists(Guid genreId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(genreId.ToString()))
            {
                return false;
            }

            var genre = await _mediator.Send(new GetGenreCommand { Id = genreId }, token);
            return genre != null;
        }
    }
}