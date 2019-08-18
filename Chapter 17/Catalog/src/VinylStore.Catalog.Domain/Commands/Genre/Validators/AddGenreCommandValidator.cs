using FluentValidation;

namespace VinylStore.Catalog.Domain.Commands.Genre.Validators
{
    public class AddGenreCommandValidator : AbstractValidator<AddGenreCommand>
    {
        public AddGenreCommandValidator()
        {
            RuleFor(genre => genre.GenreDescription).NotEmpty();
        }
    }
}
