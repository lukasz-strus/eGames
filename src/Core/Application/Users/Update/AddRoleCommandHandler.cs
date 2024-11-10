using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Update;

internal sealed class AddRoleCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddRoleCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Result.Failure<Unit>(Errors.Users.GetUserById.UserNotFound(request.UserId));

        var role = UserRole.FromValue(request.RoleId);
        if (role is null)
            return Result.Failure<Unit>(Errors.Users.AddRole.RoleNotFound(request.RoleId));

        user.AddRole(role);

        await userRepository.UpdateUserRolesAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Unit.Value);
    }
}