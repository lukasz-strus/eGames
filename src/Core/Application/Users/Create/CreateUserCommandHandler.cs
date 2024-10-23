using Application.Core.Abstractions.Data;
using Domain.Users;
using MediatR;

namespace Application.Users.Create;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, UserId>
{
    public async Task<UserId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = Customer.Create(request.User.UserName);

        await userRepository.AddCustomerAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}