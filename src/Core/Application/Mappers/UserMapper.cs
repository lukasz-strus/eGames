using Application.Contracts.User;
using Domain.Users;

namespace Application.Mappers;

internal static class UserMapper
{
    internal static UserResponse ToResponse(this User user) =>
        new(user.Id.Value,
            user.UserName,
            new UserRoleListResponse(
            [
                ..user.Roles.Select(ur => ur.ToResponse()).OrderBy(x => x.Id)
            ]));

    internal static UserRoleResponse ToResponse(this UserRole userRole) =>
        new(userRole.Value, userRole.Name);
}