using Application.Contracts.Games;
using Application.Core.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get.Game;

internal sealed class GetGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetGameByIdQuery, Result<GameResponse>>
{
    public async Task<Result<GameResponse>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        return game is not null
            ? Result.Success(game.ToResponse())
            : Result.Failure<GameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));
    }
}