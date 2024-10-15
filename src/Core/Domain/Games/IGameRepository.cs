namespace Domain.Games;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync(CancellationToken cancellationToken);
}