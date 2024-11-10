using Application.Contracts.User;
using Application.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Get;

internal sealed class GetUserQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(new UserId(request.UserId), cancellationToken);
        return user is not null
            ? Result.Success(user.ToResponse())
            : Result.Failure<UserResponse>(Errors.Users.GetUserById.UserNotFound(request.UserId));
    }
}