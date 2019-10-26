using FluentValidation;

namespace Catalog.Domain.Requests.Genre.Validators
{
    public class AddGenreRequestValidator : AbstractValidator<AddGenreRequest>
    {
        public AddGenreRequestValidator()
        {
            RuleFor(genre => genre.GenreDescription).NotEmpty();
        }
    }
}