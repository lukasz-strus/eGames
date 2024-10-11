using Domain.Customers;
using Domain.Games;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<OrderItem> _items = [];

    private Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public static Order Create(Customer customer)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id
        };

        return order;
    }

    public void Add(Game game)
    {
        var orderItem = new OrderItem(Guid.NewGuid(), Id, game.Id, game.Price);

        _items.Add(orderItem);
    }
}

public class OrderItem
{
    internal OrderItem(Guid id, Guid orderId, Guid gameId, Money price)
    {
        Id = id;
        OrderId = orderId;
        GameId = gameId;
        Price = price;
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid GameId { get; private set; }

    public Money Price { get; set; }
}
