using Application.Contracts.Games;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll;

internal sealed class GetAllGamesQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllGamesQuery, GameListResponse>
{
    public async Task<GameListResponse> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await gameRepository.GetAllAsync(cancellationToken);

        return new GameListResponse(games
            .Select(g => new GameResponse(
                g.Id.Value,
                g.Name,
                g.Description,
                g.Price.Currency,
                g.Price.Amount,
                g.ReleaseDate,
                g.Publisher))
            .ToList());
    }
}