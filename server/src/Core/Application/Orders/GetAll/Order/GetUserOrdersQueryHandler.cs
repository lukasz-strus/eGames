using Application.Authentication;
using Application.Contracts.Orders;
using Domain.Core.Results;
using Domain.Orders;
using Domain.Users;
using MediatR;

namespace Application.Orders.GetAll.Order;

internal sealed class GetUserOrdersQueryHandler(
    IOrderRepository orderRepository,
    IUserContext userContext) : IRequestHandler<GetUserOrdersQuery, Result<OrderListResponse>>
{
    public async Task<Result<OrderListResponse>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? userContext.GetCurrentUser().DomainUserId;

        var orders = await orderRepository.GetAllByUserIdAsync(new UserId(userId), cancellationToken);

        return Result.Success(new OrderListResponse(
            orders
                .Select(order => new OrderResponse(
                    order.Id.Value,
                    order.UserId.Value,
                    order.Status?.Name ?? string.Empty,
                    order.Items
                        .Select(item => new OrderItemResponse(
                            item.Id.Value,
                            item.GameId.Value,
                            item.Price.Currency.Code,
                            item.Price.Amount))
                        .ToList()))
                .ToList()));
    }
}