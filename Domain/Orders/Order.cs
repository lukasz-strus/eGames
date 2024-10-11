﻿using Domain.Users;
using Domain.Games;

namespace Domain.Orders;

public class Order
{
    private readonly HashSet<OrderItem> _items = [];

    private Order()
    {
    }

    public OrderId Id { get; private set; }

    public UserId UserId { get; private set; }

    public static Order Create(UserId userId)
    {
        var order = new Order
        {
            Id = new OrderId(Guid.NewGuid()),
            UserId = userId
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
