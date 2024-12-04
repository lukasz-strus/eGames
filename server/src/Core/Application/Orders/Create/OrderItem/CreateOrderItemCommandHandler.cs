using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Create.OrderItem;

internal sealed class CreateOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IGameRepository gameRepository,
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

        var game = await gameRepository.GetByIdAsync(new GameId(request.OrderItem.GameId), cancellationToken);
        if (game is null)
            return Result.Failure<Unit>(
                Errors.Games.GetGameById.GameNotFound(request.OrderItem.GameId));

        order.AddItem(
            game.Id,
            game.Price.Amount,
            game.Price.Currency);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}