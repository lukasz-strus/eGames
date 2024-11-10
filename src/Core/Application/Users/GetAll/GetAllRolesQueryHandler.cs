﻿using Application.Contracts.User;
using Application.Mappers;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.GetAll;

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