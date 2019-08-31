using FluentValidation;

namespace VinylStore.Catalog.Domain.Commands.Users.Validators
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
