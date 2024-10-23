namespace Domain.Users;

public class SuperAdmin : User
{
    private SuperAdmin()
    {
    }

    public static SuperAdmin Create(string userName)
    {
        var superAdmin = new SuperAdmin
        {
            Id = new UserId(Guid.NewGuid()),
            UserName = userName
        };

        return superAdmin;
    }
}