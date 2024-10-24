using Application.Core.Abstractions.Data;
using Domain.Orders;
using Domain.Users;
using MediatR;

namespace Application.Orders.Create;

internal sealed class CreateOrderCommandHandler(
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand>
{
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await userRepository.GetAsync(new UserId(request.CustomerId), cancellationToken);

        var order = Order.Create(customer.Id);

        await orderRepository.AddAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}