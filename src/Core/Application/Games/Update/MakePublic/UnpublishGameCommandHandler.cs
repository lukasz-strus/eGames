using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Update.MakePublic;

internal sealed class UnpublishGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UnpublishGameCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UnpublishGameCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<Unit>(Errors.Games.GetGameById.GameNotFound(request.Id));

        game.Unpublish();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}