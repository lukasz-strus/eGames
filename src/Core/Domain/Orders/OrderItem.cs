using Domain.Core.Primitives;
using Domain.Games;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Orders;

public class OrderItem : Entity<OrderItemId>
{
    private OrderItem()
    {
        OrderId = default!;
        GameId = default!;
        Price = default!;
    }

    public OrderItem(OrderItemId id,
        OrderId orderId,
        GameId gameId,
        Money price) : base(id)
    {
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }

    [Required] public OrderId OrderId { get; private set; }

    [Required] public GameId GameId { get; private set; }

    [Required] public Money Price { get; set; }
}