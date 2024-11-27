namespace Application.Contracts.Games;

public sealed class CreateSubscriptionRequest(
    string name,
    string description,
    decimal price,
    int currencyId,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize,
    string imageUrl,
    uint periodInDays)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal Price { get; set; } = price;
    public int CurrencyId { get; set; } = currencyId;
    public DateTime ReleaseDate { get; set; } = releaseDate;
    public string Publisher { get; set; } = publisher;
    public string DownloadLink { get; set; } = downloadLink;
    public ulong FileSize { get; set; } = fileSize;
    public string ImageUrl { get; set; } = imageUrl;
    public uint PeriodInDays { get; set; } = periodInDays;
}