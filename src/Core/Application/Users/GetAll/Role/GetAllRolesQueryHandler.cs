using Application.Contracts.User;
using Application.Internals.Mappers;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.GetAll.Role;

internal sealed class GetAllRolesQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetAllRolesQuery, Result<UserRoleListResponse>>
{
    public async Task<Result<UserRoleListResponse>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await userRepository.GetAllRolesAsync(cancellationToken);

        return new UserRoleListResponse(
            [
                ..roles.Select(r => r.ToResponse())
            ]
        );
    }
}