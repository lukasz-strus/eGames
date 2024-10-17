using Application.Core.Abstractions.Data;
using Domain.Core.Primitives;
using Domain.Orders;
using Domain.Users;
using MediatR;

namespace Application.Orders.Create;

internal sealed class CreateOrderCommandHandler(
    IUserRepository UserRepository,
    IOrderRepository OrderRepository,
    IUnitOfWork UnitOfWork) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await UserRepository.GetAsync(new UserId(request.CustomerId), cancellationToken);

        var order = Order.Create(customer.Id);

        await OrderRepository.AddAsync(order, cancellationToken);

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}