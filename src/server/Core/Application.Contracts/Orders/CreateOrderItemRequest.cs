namespace Application.Contracts.Orders;

public sealed class CreateOrderItemRequest(
    Guid gameId,
    decimal price,
    int currencyId)
{
    public Guid GameId { get; set; } = gameId;
    public decimal Price { get; set; } = price;
    public int CurrencyId { get; set; } = currencyId;
}