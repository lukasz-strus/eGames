using Domain.Users;

namespace Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<List<Order>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken);
    Task<List<OrderItem>> GetOrderItems(OrderId id, CancellationToken cancellationToken);
    Task<OrderItem?> GetOrderItemByIdAsync(OrderItemId id, CancellationToken cancellationToken);
    void Delete(Order order);
}