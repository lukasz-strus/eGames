namespace Domain.Games;

public interface IGameRepository
{
    IQueryable<Game> GetAll();
    IQueryable<FullGame> GetAllFullGames();
    IQueryable<DlcGame> GetAllDlcGames(GameId fullGameId);
    IQueryable<Subscription> GetAllSubscriptions();

    Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<Game?> GetByName(string value, CancellationToken cancellationToken);
    Task<FullGame?> GetFullGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<DlcGame?> GetDlcGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<Subscription?> GetSubscriptionByIdAsync(GameId id, CancellationToken cancellationToken);

    Task AddAsync(Game game, CancellationToken cancellationToken);
    void Delete(Game game);
}