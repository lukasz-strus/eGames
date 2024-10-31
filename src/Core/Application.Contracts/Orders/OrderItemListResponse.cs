namespace Application.Contracts.Orders;

public sealed class OrderItemListResponse(IReadOnlyCollection<OrderItemResponse> items)
{
    public IReadOnlyCollection<OrderItemResponse> Items { get; } = items;
}