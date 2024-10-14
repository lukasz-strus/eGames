using Domain.Orders;

namespace Infrastructure.Repositories;

internal sealed class OrderRepository(
    ApplicationDbContext DbContext) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await DbContext.Orders.AddAsync(order, cancellationToken);
    }
}
