namespace Domain.Users;

public class Admin : User
{
    private Admin()
    {
    }

    public static Admin Create(string userName)
    {
        var admin = new Admin
        {
            Id = new UserId(Guid.NewGuid()),
            UserName = userName
        };

        return admin;
    }
}