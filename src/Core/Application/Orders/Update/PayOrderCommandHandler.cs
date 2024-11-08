using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Libraries;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Update;

internal sealed class PayOrderCommandHandler(
    IOrderRepository orderRepository,
    ILibraryRepository libraryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PayOrderCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(PayOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
            return Result.Failure<Unit>(
                Errors.Orders.GetOrderById.OrderNotFound(request.OrderId));

        var libraryGames = order.CompleteOrder();

        foreach (var libraryGame in libraryGames)
        {
            await libraryRepository.AddAsync(libraryGame, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}