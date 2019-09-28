using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace SampleAPI.Requests
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        private readonly IList<string> _acceptedCurrencyCodes = new List<string>{
            "EUR",
            "USD",
            "GBP"
        };

        public OrderRequestValidator()
        {
            RuleFor(_ => _.ItemsIds).NotEmpty().NotNull()
                .WithMessage("should not be empty");
            RuleFor(_ => _.Currency).NotEmpty().NotNull()
                .WithMessage("should not be empty");
            RuleFor(_ => _.Currency).Must(BeValidCurrencyCode)
                .WithMessage("should be a supported currency code");
        }

        bool BeValidCurrencyCode(string currencyCode)
        {
            return _acceptedCurrencyCodes.Any(_ => _ == currencyCode);
        }
    }
}