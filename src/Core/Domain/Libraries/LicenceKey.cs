namespace Domain.Libraries;

public record LicenceKey
{
    private LicenceKey(string value) => Value = value;

    public string Value { get; init; }

    public static LicenceKey Create(string? value = null)
    {
        if (value is not null) return new LicenceKey(value);

        var key = Guid.NewGuid();

        return new LicenceKey(key.ToString());
    }
}