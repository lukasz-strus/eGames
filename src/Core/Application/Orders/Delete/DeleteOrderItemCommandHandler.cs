using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Delete;

internal sealed class DeleteOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderItemCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
            return Result.Failure<Unit>(Errors.Orders.GetOrderItemById.OrderItemNotFound(request.OrderId));

        order.RemoveItem(new OrderItemId(request.OrderItemId));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}