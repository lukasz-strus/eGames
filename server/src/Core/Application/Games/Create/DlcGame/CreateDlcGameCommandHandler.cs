﻿using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.Games;
using MediatR;

namespace Application.Games.Create.DlcGame;

internal sealed class CreateDlcGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateDlcGameCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(
        CreateDlcGameCommand request,
        CancellationToken cancellationToken)
    {
        var currency = Currency.FromValue(request.Game.CurrencyId);

        if (currency is null)
            return Result.Failure<EntityCreatedResponse>(
                Errors.Currency.FromValue.InvalidCurrencyValue(request.Game.CurrencyId));

        var game = Domain.Games.DlcGame.Create(
            request.Game.Name,
            request.Game.Description,
            request.Game.Price,
            currency,
            request.Game.ReleaseDate,
            request.Game.Publisher,
            request.Game.DownloadLink,
            request.Game.FileSize,
            request.Game.ImageUrl,
            new GameId(request.FullGameId));

        await gameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(game.Id.Value));
    }
}