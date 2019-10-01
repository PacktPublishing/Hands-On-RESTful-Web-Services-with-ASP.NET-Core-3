using FluentValidation;

namespace Catalog.Domain.Requests.Artists.Validators
{
    public class AddArtistCommandValidator : AbstractValidator<AddArtistRequest>
    {
        public AddArtistCommandValidator()
        {
            RuleFor(artist => artist.ArtistName).NotEmpty();
        }
    }
}