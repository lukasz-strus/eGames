using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Delete;

internal sealed class DeleteOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
            return Result.Failure<Unit>(Errors.Orders.GetOrderById.OrderNotFound(request.OrderId));

        orderRepository.Delete(order);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}