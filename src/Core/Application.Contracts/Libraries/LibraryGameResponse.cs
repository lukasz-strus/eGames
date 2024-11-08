namespace Application.Contracts.Libraries;

public sealed class LibraryGameResponse(
    Guid id,
    Guid gameId,
    string name)
{
    public Guid Id { get; } = id;
    public Guid GameId { get; } = gameId;
    public string Name { get; } = name;
}