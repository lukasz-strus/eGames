using Application.Contracts.Games;
using Application.Core.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Application.Games.GetAll.Game;

internal sealed class GetAllGamesQueryHandler(
    IGameRepository gameRepository,
    ISieveProcessor sieveProcessor) : IRequestHandler<GetAllGamesQuery, Result<GameListResponse>>
{
    public async Task<Result<GameListResponse>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var gamesQuery = gameRepository.GetAll();

        var games = await sieveProcessor
            .Apply(request.Query, gamesQuery)
            .ToListAsync(cancellationToken);

        var totalCount = await sieveProcessor
            .Apply(request.Query, gamesQuery, applyFiltering: false, applySorting: false)
            .CountAsync(cancellationToken);

        var gameListResponse = new GameListResponse(
            [
                .. games.Select(g => g.ToResponse()).OrderBy(x => x.Type)
            ],
            totalCount,
            request.Query.PageSize,
            request.Query.Page);

        return Result.Success(gameListResponse);
    }
}