using Domain.Users;

namespace Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<List<Order>> GetAllByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken);
    Task<List<OrderItem>> GetAllOrderItems(OrderId id, CancellationToken cancellationToken);
    Task<OrderItem?> GetOrderItemByIdAsync(OrderItemId id, CancellationToken cancellationToken);
    void Delete(Order order);
}