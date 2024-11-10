using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Update;

internal sealed class BanUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BanUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Result.Failure<Unit>(Errors.Users.GetUserById.UserNotFound(request.UserId));

        user.Ban();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}