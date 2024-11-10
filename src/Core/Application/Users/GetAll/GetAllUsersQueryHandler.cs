﻿using Application.Contracts.User;
using Application.Mappers;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, Result<UserListResponse>>
{
    public async Task<Result<UserListResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);

        return new UserListResponse(
            [
                ..users.Select(u => u.ToResponse())
            ]
        );
    }
}