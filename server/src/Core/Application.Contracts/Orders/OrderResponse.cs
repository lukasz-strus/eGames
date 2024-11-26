namespace Application.Contracts.Orders;

public sealed class OrderResponse(
    Guid id,
    Guid userId,
    string status,
    IEnumerable<OrderItemResponse> orderItems)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public string Status { get; } = status;
    public IEnumerable<OrderItemResponse> OrderItems { get; } = orderItems;
}