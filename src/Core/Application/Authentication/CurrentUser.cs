using Domain.Users;

namespace Application.Authentication;

public sealed class CurrentUser(Guid id, string email, Guid domainUserId, IEnumerable<UserRole> roles)
{
    private readonly HashSet<UserRole> _roles = [..roles];

    public Guid Id { get; } = id;
    public string Email { get; } = email;
    public Guid DomainUserId { get; } = domainUserId;
    public bool IsInRole(UserRole role) => _roles.Contains(role);
}