using Application.Contracts.Orders;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Get;

internal sealed class GetOrderItemByIdQueryHandler(
    IOrderRepository orderRepository) : IRequestHandler<GetOrderItemByIdQuery, Result<OrderItemResponse>>
{
    public async Task<Result<OrderItemResponse>> Handle(
        GetOrderItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var orderItem = await orderRepository.GetOrderItemByIdAsync(
            new OrderItemId(request.OrderItemId),
            cancellationToken);

        if (orderItem is null)
            return Result.Failure<OrderItemResponse>(
                Errors.Orders.GetOrderItemById.OrderItemNotFound(request.OrderItemId));

        return Result.Success(new OrderItemResponse(
            orderItem.Id.Value,
            orderItem.GameId.Value,
            orderItem.Price.Currency.Code,
            orderItem.Price.Amount));
    }
}