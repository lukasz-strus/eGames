using Domain.Users;
using Domain.Games;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.ValueObjects;
using Domain.Core.Primitives;
using Domain.Enums;

namespace Domain.Orders;

public class Order : Entity<OrderId>
{
    private readonly List<OrderItem> _items = [];

    private Order()
    {
        UserId = default!;
    }

    [Required] public UserId UserId { get; private set; }

    [Required] public int StatusId { get; private set; }
    [NotMapped] public OrderStatus? Status => OrderStatus.FromValue(StatusId);

    public IReadOnlyList<OrderItem> Items => _items;

    public static Order Create(UserId userId)
    {
        var order = new Order
        {
            Id = new OrderId(Guid.NewGuid()),
            UserId = userId,
            StatusId = OrderStatus.Pending.Value
        };

        return order;
    }

    public void AddItem(
        GameId gameId,
        decimal price,
        Currency currency)
    {
        var orderItem = new OrderItem(
            new OrderItemId(Guid.NewGuid()),
            Id,
            gameId,
            new Money(currency, price));

        _items.Add(orderItem);
    }

    public void RemoveItem(
        OrderItemId orderItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == orderItemId);

        if (item is null) return;

        _items.Remove(item);
    }

    public void Pay()
    {
        StatusId = OrderStatus.Paid.Value;
    }
}