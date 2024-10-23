namespace Application.Contracts.User;

public sealed class CreateUserRequest(string userName)
{
    public string UserName { get; set; } = userName;
}