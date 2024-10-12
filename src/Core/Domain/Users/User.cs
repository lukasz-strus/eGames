namespace Domain.Users;

public abstract class User
{

    public string Email { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;
    public UserId Id { get; private set; }

    public string LastName { get; private set; } = string.Empty;

    public string Login { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public abstract Role Role { get; }

}

public enum Role
{
    Customer,
    Admin
}