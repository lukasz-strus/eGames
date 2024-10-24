using Application.Contracts.Games;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get;

internal sealed class GetGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetGameByIdQuery, Result<GameResponse>>
{
    public async Task<Result<GameResponse>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<GameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));

        var gameResponse = new GameResponse(
            game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.ToString(),
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher);

        return Result.Success(gameResponse);
    }
}