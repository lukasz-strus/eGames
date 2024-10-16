using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObjects;

[ComplexType]
public record Money
{
    private const string AmountCannotBeNegative = "Amount cannot be negative";

    [Required] public Currency Currency { get; private set; }

    [Required] [Precision(18, 3)] public decimal Amount { get; private set; }

    public Money(Currency currency, decimal amount)
    {
        if (amount < 0) throw new ArgumentException(AmountCannotBeNegative, nameof(amount));

        Currency = currency;
        Amount = amount;
    }

    public override string ToString() => Currency.Format(Amount);
}