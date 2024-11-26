namespace Application.Contracts.User;

public sealed class AddRoleToUserRequest(
    int roleId)
{
    public int RoleId { get; } = roleId;
}