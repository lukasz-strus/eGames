namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(UserId id, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(UserId userId, CancellationToken cancellationToken);
}