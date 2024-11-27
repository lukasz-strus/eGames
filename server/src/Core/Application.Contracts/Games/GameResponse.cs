namespace Application.Contracts.Games;

public class GameResponse(
    Guid id,
    string type,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize,
    string imageUrl)
{
    public Guid Id { get; } = id;
    public string Type { get; } = type;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Currency { get; } = currency;
    public decimal Amount { get; } = amount;
    public DateTime ReleaseDate { get; } = releaseDate;
    public string Publisher { get; } = publisher;
    public string DownloadLink { get; } = downloadLink;
    public ulong FileSize { get; } = fileSize;
    public string ImageUrl { get; } = imageUrl;
}