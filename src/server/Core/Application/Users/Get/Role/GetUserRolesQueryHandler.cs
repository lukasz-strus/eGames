using Application.Contracts.User;
using Application.Internals.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Get.Role;

internal sealed class GetUserRolesQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetUserRolesQuery, Result<UserRoleListResponse>>
{
    public async Task<Result<UserRoleListResponse>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);

        return user is null
            ? Result.Failure<UserRoleListResponse>(Errors.Users.GetUserById.UserNotFound(request.UserId))
            : Result.Success(new UserRoleListResponse(
                [
                    ..user.Roles.Select(r => r.ToResponse()).OrderBy(x => x.Id)
                ]
            ));
    }
}