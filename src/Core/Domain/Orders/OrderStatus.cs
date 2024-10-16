using Domain.Core.Primitives;

namespace Domain.Orders;

public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(0, nameof(Pending));
    public static readonly OrderStatus Paid = new(1, nameof(Paid));
    public static readonly OrderStatus Canceled = new(2, nameof(Canceled));

    private OrderStatus(int value, string name) : base(value, name)
    {
    }
}