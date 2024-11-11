using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Delete.Role;

internal sealed class RemoveRoleCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveRoleCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Result.Failure<Unit>(Errors.Users.GetUserById.UserNotFound(request.UserId));

        var role = UserRole.FromValue(request.RoleId);
        if (role is null)
            return Result.Failure<Unit>(Errors.Users.AddRole.RoleNotFound(request.RoleId));

        user.RemoveRole(role);

        await userRepository.UpdateUserRolesAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}