using Domain.Users;
using Domain.Games;
using Domain.Primitives.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<OrderItem> _items = [];

    private Order()
    {
    }

    [Key]
    public OrderId Id { get; private set; }

    [Required]
    public UserId CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyList<OrderItem> Items => _items.ToList();

    public static Order Create(UserId customerId)
    {
        var order = new Order
        {
            Id = new OrderId(Guid.NewGuid()),
            CustomerId = customerId,
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
