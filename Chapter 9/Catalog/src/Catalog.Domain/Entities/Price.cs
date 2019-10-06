using System;

namespace Catalog.Domain.Entities
{
    public class Price
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public static Price operator +(Price a, Price b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot perform arithmetic on Money of two different types.");

            return new Price { Amount = a.Amount + b.Amount, Currency = a.Currency };
        }

        public static Price operator *(Price a, int b)
        {
            return new Price { Amount = a.Amount * b, Currency = a.Currency };
        }
    }
}