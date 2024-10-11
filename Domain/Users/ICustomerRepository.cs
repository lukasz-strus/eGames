namespace Domain.Users;

public interface ICustomerRepository
{
    Task<UserId> GetByIdAsync(UserId id);
}
