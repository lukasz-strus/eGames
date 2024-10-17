namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}