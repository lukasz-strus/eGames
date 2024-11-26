using Application.Contracts.Games;
using Application.Internals.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.Get.DlcGame;

internal sealed class GetDlcGameByIdQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetDlcGameByIdQuery, Result<DlcGameResponse>>
{
    public async Task<Result<DlcGameResponse>> Handle(GetDlcGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetDlcGameByIdAsync(new GameId(request.Id), cancellationToken);

        return game is not null
            ? Result.Success(game.ToResponse())
            : Result.Failure<DlcGameResponse>(Errors.Games.GetGameById.GameNotFound(request.Id));
    }
}