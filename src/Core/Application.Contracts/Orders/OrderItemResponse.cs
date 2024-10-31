namespace Application.Contracts.Orders;

public sealed class OrderItemResponse(
    Guid id,
    Guid gameId,
    string currency,
    decimal amount)
{
    public Guid Id { get; } = id;
    public Guid GameId { get; } = gameId;
    public string Currency { get; } = currency;
    public decimal Amount { get; } = amount;
}