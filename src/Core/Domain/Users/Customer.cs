namespace Domain.Users;

public class Customer : User
{
    private Customer()
    {
    }

    public static Customer Create(
        string login,
        string email,
        string firstName,
        string lastName,
        string phoneNumber)
    {
        var customer = new Customer
        {
            Id = new UserId(Guid.NewGuid()),
            Login = login,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber
        };

        return customer;
    }
}