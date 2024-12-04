using Application.Authentication;
using Application.Contracts.Libraries;
using Domain.Core.Results;
using Domain.Libraries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Application.Libraries.GetAll;

internal sealed class GetAllLibraryGamesQueryHandler(
    ILibraryRepository libraryRepository,
    IUserContext userContext,
    ISieveProcessor sieveProcessor) : IRequestHandler<GetAllLibraryGamesQuery, Result<LibraryGameListResponse>>
{
    public async Task<Result<LibraryGameListResponse>> Handle(
        GetAllLibraryGamesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? userContext.GetCurrentUser().DomainUserId;

        var libraryGamesQuery = libraryRepository.GetAllByUserId(new UserId(userId));

        var libraryGames = await sieveProcessor
            .Apply(request.Query, libraryGamesQuery)
            .ToListAsync(cancellationToken);

        var totalCount = await sieveProcessor
            .Apply(request.Query, libraryGamesQuery, applyPagination: false)
            .CountAsync(cancellationToken);

        return Result.Success(new LibraryGameListResponse(
            [
                ..libraryGames.Select(libraryGame => new LibraryGameResponse(
                    libraryGame.Id.Value,
                    libraryGame.GameId.Value,
                    libraryGame.Game.Name,
                    libraryGame.Game.ImageUrl))
            ],
            totalCount,
            request.Query.PageSize,
            request.Query.Page));
    }
}