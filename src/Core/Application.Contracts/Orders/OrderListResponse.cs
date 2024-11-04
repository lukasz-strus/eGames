using System.Collections.Generic;

namespace Application.Contracts.Orders;

public sealed class OrderListResponse(IReadOnlyCollection<OrderResponse> orders)
{
    public IReadOnlyCollection<OrderResponse> Orders { get; } = orders;
}