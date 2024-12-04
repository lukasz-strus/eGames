namespace Application.Contracts.Libraries;

public sealed class LibraryGameResponse(
    Guid id,
    Guid gameId,
    string name,
    string imageUrl)
{
    public Guid Id { get; } = id;
    public Guid GameId { get; } = gameId;
    public string Name { get; } = name;
    public string ImageUrl { get; } = imageUrl;
}