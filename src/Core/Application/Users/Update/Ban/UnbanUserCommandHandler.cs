using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Update.Ban;

internal sealed class UnbanUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UnbanUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Result.Failure<Unit>(Errors.Users.GetUserById.UserNotFound(request.UserId));

        user.Unban();

        await userRepository.UpdateUserRolesAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}