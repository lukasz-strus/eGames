using Domain.Core.Primitives;

namespace Domain.Orders;

public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(0, nameof(Pending));
    public static readonly OrderStatus Paid = new(1, nameof(Paid));

    private OrderStatus(int value, string name) : base(value, name)
    {
    }
}