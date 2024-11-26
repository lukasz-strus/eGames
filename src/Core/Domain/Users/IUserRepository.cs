namespace Domain.Users;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<UserRole>> GetAllRolesAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);
    Task<User?> GetByUserName(string userName, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(UserId userId, CancellationToken cancellationToken);

    Task UpdateUserRolesAsync(User user, CancellationToken cancellationToken);
}