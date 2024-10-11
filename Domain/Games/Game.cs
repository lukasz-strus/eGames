namespace Domain.Games;

public class Game
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; }
}