using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace SampleAPI.Requests
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        private readonly IEnumerable<string> _acceptedCurrencyCodes = new List<string>{
            "EUR",
            "USD",
            "GBP"
        };

        public OrderRequestValidator()
        {
            RuleFor(x => x.ItemsIds).NotEmpty().NotNull()
                .WithMessage("should not be empty");
            RuleFor(x => x.Currency).NotEmpty().NotNull()
                .WithMessage("should not be empty");
            RuleFor(x => x.Currency).Must(BeValidCurrencyCode)
                .WithMessage("should be a supported currency code");
        }

        bool BeValidCurrencyCode(string currencyCode)
        {
            return _acceptedCurrencyCodes.Any(x => x == currencyCode);
        }
    }
}