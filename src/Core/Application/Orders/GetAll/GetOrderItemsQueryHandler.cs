using Application.Contracts.Orders;
using Domain;
using Domain.Core.Results;
using Domain.Orders;
using MediatR;

namespace Application.Orders.GetAll;

internal sealed class GetOrderItemsQueryHandler(
    IOrderRepository orderRepository) : IRequestHandler<GetOrderItemsQuery, Result<OrderItemListResponse>>
{
    public async Task<Result<OrderItemListResponse>> Handle(GetOrderItemsQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetOrderItems(new OrderId(request.OrderId), cancellationToken);

        return Result.Success(new OrderItemListResponse(
            orders.Select(orderItem => new OrderItemResponse(
                    orderItem.Id.Value,
                    orderItem.GameId.Value,
                    orderItem.Price.Currency.Code,
                    orderItem.Price.Amount))
                .ToList()));
    }
}