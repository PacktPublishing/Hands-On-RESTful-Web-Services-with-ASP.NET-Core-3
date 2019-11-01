using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Services;
using FluentValidation;

namespace Catalog.Domain.Requests.Item.Validators
{
    public class EditItemRequestValidator : AbstractValidator<EditItemRequest>
    {
        private readonly IArtistService _artistService;
        private readonly IGenreService _genreService;

        public EditItemRequestValidator(IArtistService artistService, IGenreService genreService)
        {
            _genreService = genreService;
            _artistService = artistService;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.GenreId)
                .NotEmpty()
                .MustAsync(GenreExists).WithMessage("Genre must exists");
            RuleFor(x => x.ArtistId)
                .NotEmpty()
                .MustAsync(ArtistExists).WithMessage("Artist must exists");
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.ReleaseDate).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }

        private async Task<bool> ArtistExists(Guid artistId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(artistId.ToString())) return false;

            var artist = await _artistService.GetArtistAsync(new GetArtistRequest {Id = artistId});
            return artist != null;
        }

        private async Task<bool> GenreExists(Guid genreId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(genreId.ToString())) return false;

            var genre = await _genreService.GetGenreAsync(new GetGenreRequest {Id = genreId});
            return genre != null;
        }
    }
}