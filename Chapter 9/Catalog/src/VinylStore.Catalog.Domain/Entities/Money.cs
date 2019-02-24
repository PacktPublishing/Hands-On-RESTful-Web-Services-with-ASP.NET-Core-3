using System;

namespace VinylStore.Catalog.Domain.Entities
{
    public class Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot perform arithmetic on Money of two different types.");

            return new Money {Amount = a.Amount + b.Amount, Currency = a.Currency};
        }

        public static Money operator *(Money a, int b)
        {
            return new Money {Amount = a.Amount * b, Currency = a.Currency};
        }
    }
}