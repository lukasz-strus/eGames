using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Create.User;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = Domain.Users.User.Create(request.User.UserName);

        await userRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(user.Id.Value));
    }
}