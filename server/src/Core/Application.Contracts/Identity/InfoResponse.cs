using Application.Contracts.User;

namespace Application.Contracts.Identity;

public sealed class InfoCustomResponse
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required bool IsEmailConfirmed { get; init; }
    public required UserRoleListResponse UserRoles { get; init; }
}