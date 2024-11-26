using Application.Authentication;
using Application.Contracts.Libraries;
using Domain.Core.Results;
using Domain.Libraries;
using Domain.Users;
using MediatR;

namespace Application.Libraries.GetAll;

internal sealed class GetAllLibraryGamesQueryHandler(
    ILibraryRepository libraryRepository,
    IUserContext userContext) : IRequestHandler<GetAllLibraryGamesQuery, Result<LibraryGameListResponse>>
{
    public async Task<Result<LibraryGameListResponse>> Handle(
        GetAllLibraryGamesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? userContext.GetCurrentUser().DomainUserId;

        var libraryGames = await libraryRepository.GetAllByUserIdAsync(new UserId(userId), cancellationToken);

        return Result.Success(new LibraryGameListResponse(
            libraryGames
                .Select(libraryGame => new LibraryGameResponse(
                    libraryGame.Id.Value,
                    libraryGame.GameId.Value,
                    libraryGame.Game.Name))
                .ToList()));
    }
}