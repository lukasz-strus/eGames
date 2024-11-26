using Application.Contracts.Orders;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Get.Order;

internal sealed class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.Id), cancellationToken);

        if (order is null)
            return Result.Failure<OrderResponse>(Errors.Orders.GetOrderById.OrderNotFound(request.Id));

        return Result.Success(new OrderResponse(
            order.Id.Value,
            order.UserId.Value,
            order.Status?.Name ?? string.Empty,
            order.Items.Select(orderItem => new OrderItemResponse(
                orderItem.Id.Value,
                orderItem.GameId.Value,
                orderItem.Price.Currency.Code,
                orderItem.Price.Amount))));
    }
}