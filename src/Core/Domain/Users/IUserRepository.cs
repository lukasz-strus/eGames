namespace Domain.Users;

public interface IUserRepository
{
    Task<Customer> GetCustomerAsync(UserId id, CancellationToken cancellationToken);
}
