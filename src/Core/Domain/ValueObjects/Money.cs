using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObjects;

public record Money
{
    // ReSharper disable once UnusedMember.Local
    private Money()
    {
        Currency = Currency.Pln;
        Amount = 0;
    }

    private const string AmountCannotBeNegative = "Amount cannot be negative";

    [Required] public Currency Currency { get; }

    [Required] [Precision(18, 3)] public decimal Amount { get; }

    public Money(Currency currency, decimal amount)
    {
        if (amount < 0) throw new ArgumentException(AmountCannotBeNegative, nameof(amount));

        Currency = currency;
        Amount = amount;
    }

    public override string ToString() => Currency.Format(Amount);
}