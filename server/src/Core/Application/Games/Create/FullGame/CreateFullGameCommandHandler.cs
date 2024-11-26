using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.Games;
using MediatR;

namespace Application.Games.Create.FullGame;

internal sealed class CreateFullGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateFullGameCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateFullGameCommand request,
        CancellationToken cancellationToken)
    {
        var currency = Currency.FromValue(request.Game.CurrencyId);

        if (currency is null)
            return Result.Failure<EntityCreatedResponse>(
                Errors.Currency.FromValue.InvalidCurrencyValue(request.Game.CurrencyId));

        var game = Domain.Games.FullGame.Create(
            request.Game.Name,
            request.Game.Description,
            request.Game.Price,
            currency,
            request.Game.ReleaseDate,
            request.Game.Publisher,
            request.Game.DownloadLink,
            request.Game.FileSize,
            []);

        await gameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(game.Id.Value));
    }
}