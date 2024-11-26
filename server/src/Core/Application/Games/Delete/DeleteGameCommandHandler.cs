using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Delete;

internal sealed class DeleteGameCommandHandler(
    IGameRepository gameRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteGameCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<Unit>(Errors.Games.GetGameById.GameNotFound(request.Id));

        if (request.Destroy is true)
            gameRepository.Delete(game);
        else
            game.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}