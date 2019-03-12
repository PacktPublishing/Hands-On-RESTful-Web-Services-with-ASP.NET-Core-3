using FluentValidation;

namespace VinylStore.Catalog.Domain.Commands.User.Validators
{
    public class SignUpUserRequestValidator : AbstractValidator<SignUpUserCommand>
    {
        public SignUpUserRequestValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Email).NotEmpty();
            RuleFor(_ => _.Email).EmailAddress();
            RuleFor(_ => _.Password).NotEmpty();
        }
    }
}
