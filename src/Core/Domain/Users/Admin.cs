namespace Domain.Users;

public class Admin : User
{
    private Admin()
    {
    }

    public static Admin Create()
    {
        var admin = new Admin
        {
            Id = new UserId(Guid.NewGuid())
        };

        return admin;
    }
}