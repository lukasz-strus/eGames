using Application.Contracts.Games;
using Application.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll;

internal sealed class GetAllDlcGamesQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllDlcGamesQuery, Result<DlcGameListResponse>>
{
    public async Task<Result<DlcGameListResponse>> Handle(
        GetAllDlcGamesQuery request,
        CancellationToken cancellationToken)
    {
        var dlcGames = await gameRepository.GetAllDlcGamesAsync(
            new GameId(request.FullGameId),
            request.IsPublished,
            cancellationToken);

        var dlcGameListResponse = new DlcGameListResponse(
            [
                ..dlcGames.Select(g => g.ToResponse())
            ]
        );
        return Result.Success(dlcGameListResponse);
    }
}