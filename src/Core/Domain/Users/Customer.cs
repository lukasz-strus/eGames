namespace Domain.Users;

public class Customer : User
{
    private Customer()
    {
    }

    public static Customer Create(string userName)
    {
        var customer = new Customer
        {
            Id = new UserId(Guid.NewGuid()),
            UserName = userName
        };

        return customer;
    }
}