using Application.Contracts.Games;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll;

internal sealed class GetAllGamesQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllGamesQuery, Result<GameListResponse>>
{
    public async Task<Result<GameListResponse>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await gameRepository.GetAllAsync(cancellationToken);

        var gameListResponse = new GameListResponse(games
            .Select(g => new GameResponse(
                g.Id.Value,
                g.GetType().Name,
                g.Name,
                g.Description,
                g.Price.Currency.ToString(),
                g.Price.Amount,
                g.ReleaseDate,
                g.Publisher,
                g.FileSize))
            .OrderBy(x => x.Type)
            .ToList());

        return Result.Success(gameListResponse);
    }
}