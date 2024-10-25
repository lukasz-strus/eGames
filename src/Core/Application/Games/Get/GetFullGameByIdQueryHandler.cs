using Application.Contracts.Games;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get;

internal sealed class GetFullGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetFullGameByIdQuery, Result<FullGameResponse>>
{
    public async Task<Result<FullGameResponse>> Handle(GetFullGameByIdQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetFullGameByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<FullGameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));

        var gameResponse = new FullGameResponse(
            game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.Name,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.FileSize,
            game.DlcGames.Select(x => new DlcGameResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.Price.Currency.Name,
                x.Price.Amount,
                x.ReleaseDate,
                x.Publisher,
                x.FileSize,
                x.FullGameId.Value)));

        return Result.Success(gameResponse);
    }
}