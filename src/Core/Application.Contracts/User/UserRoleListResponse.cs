namespace Application.Contracts.User;

public sealed class UserRoleListResponse(IReadOnlyCollection<UserRoleResponse> items)
{
    public IReadOnlyCollection<UserRoleResponse> Items { get; } = items;
}