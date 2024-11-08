using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class OrderRepository(
    ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken) =>
        await dbContext.Orders.AddAsync(order, cancellationToken);

    public async Task<List<Order>> GetAllByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        await dbContext.Orders
            .Include(x => x.Items)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken) =>
        await dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);


    public async Task<List<OrderItem>> GetAllOrderItems(OrderId id, CancellationToken cancellationToken) =>
        await dbContext.OrderItems
            .Where(x => x.OrderId == id)
            .ToListAsync(cancellationToken);

    public async Task<OrderItem?> GetOrderItemByIdAsync(OrderItemId id, CancellationToken cancellationToken) =>
        await dbContext.OrderItems
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Delete(Order order) =>
        dbContext.Orders.Remove(order);
}