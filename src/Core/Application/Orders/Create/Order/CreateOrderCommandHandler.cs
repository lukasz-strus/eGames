using Application.Authentication;
using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain.Core.Results;
using Domain.Orders;
using Domain.Users;
using MediatR;

namespace Application.Orders.Create.Order;

internal sealed class CreateOrderCommandHandler(
    IUserContext userContext,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var order = Domain.Orders.Order.Create(new UserId(currentUser.DomainUserId));

        await orderRepository.AddAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(order.Id.Value));
    }
}