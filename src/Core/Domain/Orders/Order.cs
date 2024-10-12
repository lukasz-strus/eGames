using Domain.Users;
using Domain.Games;
using Domain.Primitives.ValueObjects;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<OrderItem> _items = [];

    private Order()
    {
    }

    public OrderId Id { get; private set; }

    public UserId UserId { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public static Order Create(UserId userId)
    {
        var order = new Order
        {
            Id = new OrderId(Guid.NewGuid()),
            UserId = userId,
            Status = OrderStatus.Pending
        };

        return order;
    }

    public void Add(GameId gameId, Money price)
    {
        var orderItem = new OrderItem(
            new OrderItemId(Guid.NewGuid()), 
            Id,
            gameId,
            price);

        _items.Add(orderItem);
    }
}
