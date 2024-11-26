using Application.Core.Abstractions.Data;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Delete.User;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await userRepository.DeleteAsync(request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}