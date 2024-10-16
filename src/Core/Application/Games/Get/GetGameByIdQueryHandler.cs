using Application.Contracts.Games;
using Domain.Games;
using MediatR;

namespace Application.Games.Get;

internal sealed class GetGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetGameByIdQuery, GameResponse>
{
    public async Task<GameResponse> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        return new GameResponse(
            game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.ToString(),
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher);
    }
}