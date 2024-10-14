using Domain.Games;
using Domain.Primitives.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Orders;

public class OrderItem
{
    private OrderItem()
    {
    }

    internal OrderItem(OrderItemId id, OrderId orderId, GameId gameId, Money price)
    {
        Id = id;
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }

    [Key]
    public OrderItemId Id { get; private set; }

    [Required]
    public OrderId OrderId { get; private set; }

    [Required]
    public GameId GameId { get; private set; }

    [Required]
    public Money Price { get; set; }
}
