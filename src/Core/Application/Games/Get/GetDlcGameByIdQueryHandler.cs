using Application.Contracts.Games;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get;

internal sealed class GetDlcGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetDlcGameByIdQuery, Result<DlcGameResponse>>
{
    public async Task<Result<DlcGameResponse>> Handle(GetDlcGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetDlcGameByIdAsync(new GameId(request.Id), cancellationToken);

        if (game is null)
            return Result.Failure<DlcGameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));

        var gameResponse = new DlcGameResponse(
            game.Id.Value,
            game.Name,
            game.Description,
            game.Price.Currency.Name,
            game.Price.Amount,
            game.ReleaseDate,
            game.Publisher,
            game.FileSize,
            game.FullGameId.Value);

        return Result.Success(gameResponse);
    }
}