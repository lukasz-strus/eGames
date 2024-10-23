namespace Application.Contracts.Identity;

public sealed class RegisterRequest(string userName, string email, string password)
{
    public string UserName { get; } = userName;
    public string Email { get; } = email;
    public string Password { get; } = password;
}