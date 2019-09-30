using FluentValidation;

namespace Catalog.Domain.Commands.Item.Validators
{
    public class EditItemCommandValidator : AbstractValidator<EditItemCommand>
    {
        public EditItemCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.GenreId).NotEmpty();
            RuleFor(x => x.ArtistId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.ReleaseDate).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}