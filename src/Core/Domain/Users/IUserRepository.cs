namespace Domain.Users;

public interface IUserRepository
{
    Task<Customer> GetCustomerAsync(UserId id, CancellationToken cancellationToken);
    Task AddCustomerAsync(Customer user, CancellationToken cancellationToken);
    Task DeleteAsync(UserId userId, CancellationToken cancellationToken);
}