using Application.Contracts.Games;
using Application.Internals.Mappers;
using Domain.Core.Results;
using Domain.Games;
using MediatR;

namespace Application.Games.GetAll.FullGame;

internal sealed class GetAllFullGamesQueryHandler(
    IGameRepository gameRepository) : IRequestHandler<GetAllFullGamesQuery, Result<FullGameListResponse>>
{
    public async Task<Result<FullGameListResponse>> Handle(GetAllFullGamesQuery request,
        CancellationToken cancellationToken)
    {
        var fullGames = await gameRepository.GetAllFullGamesAsync(
            request.IsPublished,
            cancellationToken);

        var fullGameListResponse = new FullGameListResponse(
            [
                ..fullGames.Select(g => g.ToResponse())
            ]
        );

        return Result.Success(fullGameListResponse);
    }
}