using Application.Contracts.Games;
using Application.Core.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Application.Games.GetAll.FullGame;

internal sealed class GetAllFullGamesQueryHandler(
    IGameRepository gameRepository,
    ISieveProcessor sieveProcessor) : IRequestHandler<GetAllFullGamesQuery, Result<FullGameListResponse>>
{
    public async Task<Result<FullGameListResponse>> Handle(GetAllFullGamesQuery request,
        CancellationToken cancellationToken)
    {
        var fullGamesQuery = gameRepository.GetAllFullGames();

        var fullGames = await sieveProcessor
            .Apply(request.Query, fullGamesQuery)
            .ToListAsync(cancellationToken);

        var totalCount = await sieveProcessor
            .Apply(request.Query, fullGamesQuery, applyPagination: false)
            .CountAsync(cancellationToken);

        var fullGameListResponse = new FullGameListResponse(
            [
                ..fullGames.Select(g => g.ToResponse())
            ],
            totalCount,
            request.Query.PageSize,
            request.Query.Page);

        return Result.Success(fullGameListResponse);
    }
}