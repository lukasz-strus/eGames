using Domain.Users;

namespace Application.Authentication;

public sealed class CurrentUser(Guid id, string email, Guid domainUserId)
{
    public Guid Id { get; } = id;
    public string Email { get; } = email;
    public Guid DomainUserId { get; } = domainUserId;
}