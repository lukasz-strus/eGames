namespace Domain.Users;

public class Admin : User
{
    private Admin()
    {
    }

    public static Admin Create(
        string login,
        string email,
        string firstName,
        string lastName,
        string phoneNumber)
    {
        var admin = new Admin
        {
            Id = new UserId(Guid.NewGuid()),
            Login = login,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber
        };

        return admin;
    }
}