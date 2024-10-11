namespace Domain.Users;

public class Customer : User
{
    public override Role Role => Role.Customer;
}
