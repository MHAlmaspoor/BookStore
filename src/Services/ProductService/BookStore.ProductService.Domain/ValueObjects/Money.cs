using BookStore.ProductServicec.Domain.Exceptions;
using BookStore.ProductServicec.Domain.Common;

namespace BookStore.ProductServicec.Domain.ValueObjects;

public sealed class Money:ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount,string currency)
    {
        if(amount<0)
            throw new DomainException(DomainErrors.Money.NegativeAmount);
        if(string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required. ");
        Amount=amount;
        if(currency.Trim().Length!=3)
            throw new DomainException(DomainErrors.Money.InvalidCurrency);
        Currency=currency.Trim().ToUpperInvariant();
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
