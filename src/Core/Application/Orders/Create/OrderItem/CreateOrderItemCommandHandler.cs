using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.Games;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Create.OrderItem;

internal sealed class CreateOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderItemCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        CreateOrderItemCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
            return Result.Failure<Unit>(
                Errors.Orders.GetOrderById.OrderNotFound(request.OrderId));

        var currency = Currency.FromValue(request.OrderItem.CurrencyId);

        if (currency is null)
            return Result.Failure<Unit>(
                Errors.Currency.FromValue.InvalidCurrencyValue(request.OrderItem.CurrencyId));

        order.AddItem(new GameId(request.OrderItem.GameId),
            request.OrderItem.Price,
            currency);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}