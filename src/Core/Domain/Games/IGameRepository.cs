namespace Domain.Games;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync(bool? isPublished, CancellationToken cancellationToken);
    Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<List<FullGame>> GetAllFullGamesAsync(bool? isPublished, CancellationToken cancellationToken);
    Task<List<DlcGame>> GetAllDlcGamesAsync(GameId fullGameId, bool? isPublished, CancellationToken cancellationToken);
    Task<List<Subscription>> GetAllSubscriptionsAsync(bool? isPublished, CancellationToken cancellationToken);

    Task<FullGame?> GetFullGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<DlcGame?> GetDlcGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<Subscription?> GetSubscriptionByIdAsync(GameId id, CancellationToken cancellationToken);

    Task AddAsync(Game game, CancellationToken cancellationToken);
}