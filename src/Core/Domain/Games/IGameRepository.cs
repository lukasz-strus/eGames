namespace Domain.Games;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync(CancellationToken cancellationToken);
    Task<FullGame?> GetFullGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<DlcGame?> GetDlcGameByIdAsync(GameId id, CancellationToken cancellationToken);
    Task<Subscription?> GetSubscriptionByIdAsync(GameId id, CancellationToken cancellationToken);
    Task AddAsync(Game game, CancellationToken cancellationToken);
}