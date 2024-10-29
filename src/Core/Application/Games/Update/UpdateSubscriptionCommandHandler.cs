using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.Games;
using Domain.ValueObjects;
using MediatR;

namespace Application.Games.Update;

internal sealed class UpdateSubscriptionCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSubscriptionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetSubscriptionByIdAsync(
            new GameId(request.Id),
            cancellationToken);

        if (game is null)
            return Result.Failure<Unit>(Errors.Games.GetGameById.GameNotFound(request.Id));

        var currency = Currency.FromValue(request.Game.CurrencyId);

        if (currency is null)
            return Result.Failure<Unit>(
                Errors.Currency.FromValue.InvalidCurrencyValue(request.Game.CurrencyId));

        var money = new Money(currency, request.Game.Price);

        game.Update(
            request.Game.Name,
            request.Game.Description,
            money,
            request.Game.ReleaseDate,
            request.Game.Publisher,
            request.Game.DownloadLink,
            request.Game.FileSize,
            request.Game.PeriodInDays);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}