using FluentValidation;

namespace Catalog.Domain.Requests.Item.Validators
{
    public class EditItemRequestValidator : AbstractValidator<EditItemRequest>
    {
        public EditItemRequestValidator()
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