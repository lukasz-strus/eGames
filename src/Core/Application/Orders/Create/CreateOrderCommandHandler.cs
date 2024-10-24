using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using Domain.Users;
using MediatR;

namespace Application.Orders.Create;

internal sealed class CreateOrderCommandHandler(
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(new UserId(request.CustomerId), cancellationToken);

        if (user is null)
            return Result.Failure<EntityCreatedResponse>(
                Errors.Orders.CreateOrder.CustomerNotFound(
                    request.CustomerId));

        var order = Order.Create(user.Id);

        await orderRepository.AddAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(order.Id.Value));
    }
}