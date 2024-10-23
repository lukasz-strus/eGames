namespace Application.Contracts.Games;

public class CreateGameRequest(
    string name,
    string description,
    decimal price,
    int currencyId,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal Price { get; set; } = price;
    public int CurrencyId { get; set; } = currencyId;
    public DateTime ReleaseDate { get; set; } = releaseDate;
    public string Publisher { get; set; } = publisher;
    public string DownloadLink { get; set; } = downloadLink;
    public ulong FileSize { get; set; } = fileSize;
}