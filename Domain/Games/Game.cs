namespace Domain.Games;

public class Game
{
    public GameId Id { get; private set; } 
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; }
    public DateTime ReleaseDate { get; set; }
    public string Publisher { get; set; } = string.Empty;

}
