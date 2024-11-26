using Application.Contracts.Games;
using Application.Internals.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll.Game;

internal sealed class GetAllGamesQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllGamesQuery, Result<GameListResponse>>
{
    public async Task<Result<GameListResponse>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await gameRepository.GetAllAsync(
            request.IsPublished,
            request.IsSoftDeleted,
            cancellationToken);

        var gameListResponse = new GameListResponse(
        [
            .. games.Select(g => g.ToResponse()).OrderBy(x => x.Type)
        ]);

        return Result.Success(gameListResponse);
    }
}