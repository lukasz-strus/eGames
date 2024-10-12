using Domain.Games;
using Domain.Primitives.ValueObjects;

namespace Domain.Orders;

public class OrderItem
{
    internal OrderItem(OrderItemId id, OrderId orderId, GameId gameId, Money price)
    {
        Id = id;
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }

    public OrderItemId Id { get; private set; }

    public OrderId OrderId { get; private set; }

    public GameId GameId { get; private set; }

    public Money Price { get; set; }
}
