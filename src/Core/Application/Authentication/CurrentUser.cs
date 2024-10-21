using Domain.Users;

namespace Application.Authentication;

public sealed class CurrentUser(Guid id, string email)
{
    public Guid Id { get; } = id;
    public string Email { get; } = email;
}