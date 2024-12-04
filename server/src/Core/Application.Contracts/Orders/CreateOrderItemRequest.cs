namespace Application.Contracts.Orders;

public sealed class CreateOrderItemRequest(
    Guid gameId)
{
    public Guid GameId { get; set; } = gameId;
}