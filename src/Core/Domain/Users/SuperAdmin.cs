namespace Domain.Users;

public class SuperAdmin : User
{
    private SuperAdmin()
    {
    }

    public static SuperAdmin Create()
    {
        var superAdmin = new SuperAdmin
        {
            Id = new UserId(Guid.NewGuid())
        };

        return superAdmin;
    }
}