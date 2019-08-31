using FluentValidation;

namespace VinylStore.Catalog.Domain.Commands.User.Validators
{
    public class GetUserRequestValidator : AbstractValidator<GetUserCommand>
    {
        public GetUserRequestValidator()
        {
            RuleFor(_ => _.Email).NotEmpty();
            RuleFor(_ => _.Email).EmailAddress();
        }
    }
}
