namespace Application.Contracts.User;

public sealed class UserResponse(
    Guid id,
    string userName,
    UserRoleListResponse userRoles)
{
    public Guid Id { get; } = id;
    public string UserName { get; } = userName;
    public UserRoleListResponse UserRoles { get; } = userRoles;
}