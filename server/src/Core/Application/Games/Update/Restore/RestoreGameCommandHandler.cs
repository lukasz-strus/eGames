﻿using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Update.Restore;

internal sealed class RestoreGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RestoreGameCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RestoreGameCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<Unit>(Errors.Games.GetGameById.GameNotFound(request.Id));

        game.Restore();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}