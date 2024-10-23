﻿using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain.Enums;
using Domain.Games;
using Domain.ValueObjects;
using MediatR;

namespace Application.Games.Create;

internal sealed class CreateFullGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateFullGameCommand, EntityCreatedResponse>
{
    public async Task<EntityCreatedResponse> Handle(CreateFullGameCommand request, CancellationToken cancellationToken)
    {
        var currency = Currency.FromValue(request.Game.CurrencyId)
                       ?? throw new InvalidOperationException(
                           $"The currency with ID {request.Game.CurrencyId} was not found.");

        var money = new Money(currency, request.Game.Price);

        var game = FullGame.Create(
            request.Game.Name,
            request.Game.Description,
            request.Game.Price,
            currency,
            request.Game.ReleaseDate,
            request.Game.Publisher,
            request.Game.DownloadLink,
            request.Game.FileSize);

        await gameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new EntityCreatedResponse(game.Id.Value);
    }
}