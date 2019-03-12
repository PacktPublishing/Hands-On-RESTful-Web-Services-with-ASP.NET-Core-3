using FluentValidation;

namespace VinylStore.Catalog.Domain.Commands.User.Validators
{
    public class SignInUserRequestValidator : AbstractValidator<SignInUserCommand>
    {
        public SignInUserRequestValidator()
        {
            RuleFor(_ => _.Email).NotEmpty();
            RuleFor(_ => _.Email).EmailAddress();
            RuleFor(_ => _.Password).NotEmpty();
        }
    }
}
