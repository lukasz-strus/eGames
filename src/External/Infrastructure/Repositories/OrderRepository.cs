using Domain.Orders;

namespace Infrastructure.Repositories;

internal sealed class OrderRepository(
    ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await dbContext.Orders.AddAsync(order, cancellationToken);
    }
}