namespace Domain.Users;

public class Customer : User
{
    private Customer()
    {
    }

    public static Customer Create()
    {
        var customer = new Customer
        {
            Id = new UserId(Guid.NewGuid())
        };

        return customer;
    }
}