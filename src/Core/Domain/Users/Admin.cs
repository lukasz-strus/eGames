namespace Domain.Users;

public class Admin : User
{
    public override Role Role => Role.Admin;
}
