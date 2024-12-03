using Application.Contracts.Games;
using Application.Core.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Application.Games.GetAll.DlcGame;

internal sealed class GetAllDlcGamesQueryHandler(
    IGameRepository gameRepository,
    ISieveProcessor sieveProcessor) : IRequestHandler<GetAllDlcGamesQuery, Result<DlcGameListResponse>>
{
    public async Task<Result<DlcGameListResponse>> Handle(
        GetAllDlcGamesQuery request,
        CancellationToken cancellationToken)
    {
        var dlcGamesQuery = gameRepository.GetAllDlcGames(new GameId(request.FullGameId));

        var dlcGames = await sieveProcessor
            .Apply(request.Query, dlcGamesQuery)
            .ToListAsync(cancellationToken);

        var totalCount = await sieveProcessor
            .Apply(request.Query, dlcGamesQuery, applyPagination: false)
            .CountAsync(cancellationToken);

        var dlcGameListResponse = new DlcGameListResponse(
            [
                ..dlcGames.Select(g => g.ToResponse())
            ],
            totalCount,
            request.Query.PageSize,
            request.Query.Page);

        return Result.Success(dlcGameListResponse);
    }
}