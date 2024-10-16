using Domain.Core.Primitives;

namespace Domain.Enums;

public sealed class Currency : Enumeration<Currency>
{
    public static readonly Currency Usd = new(1, "Dollar", "USD");
    public static readonly Currency Eur = new(2, "Euro", "EUR");
    public static readonly Currency Pln = new(3, "Polish zloty", "PLN");
    internal static readonly Currency None = new(default, string.Empty, string.Empty);
    public string Code { get; }

    private Currency(int value, string name, string code)
        : base(value, name)
    {
        Code = code;
    }

    public string Format(decimal amount) => $"{amount:n2} {Code}";

    public override string ToString() => Code;
}