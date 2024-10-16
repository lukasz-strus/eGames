using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObjects;

public record Money
{
    private const int CurrencyCodeLength = 3;

    [Required] [MaxLength(3)] public string Currency { get; private set; }

    [Required] [Precision(18, 3)] public decimal Amount { get; private set; }

    public Money(string currency, decimal amount)
    {
        Validate(currency, amount);

        Currency = currency;
        Amount = amount;
    }

    private static void Validate(string currency, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (currency.Length != CurrencyCodeLength)
            throw new ArgumentException($"Currency must have {CurrencyCodeLength} characters", nameof(currency));
    }
}