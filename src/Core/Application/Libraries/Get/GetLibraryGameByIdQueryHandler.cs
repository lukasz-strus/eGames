using Application.Authentication;
using Application.Contracts.Libraries;
using Application.Mappers;
using Domain;
using Domain.Core.Results;
using Domain.Libraries;
using Domain.Users;
using MediatR;

namespace Application.Libraries.Get;

internal sealed class GetLibraryGameByIdQueryHandler(
    ILibraryRepository libraryRepository,
    IUserContext userContext) : IRequestHandler<GetLibraryGameByIdQuery, Result<FullLibraryGameResponse>>
{
    public async Task<Result<FullLibraryGameResponse>> Handle(
        GetLibraryGameByIdQuery request,
        CancellationToken cancellationToken)
    {
        var libraryGame = await libraryRepository.GetByIdAsync(
            new LibraryGameId(request.LibraryGameId),
            cancellationToken);

        if (libraryGame is null)
            return Result.Failure<FullLibraryGameResponse>(
                Errors.Libraries.GetLibraryGameById.LibraryGameNotFound(request.LibraryGameId));

        var user = userContext.GetCurrentUser();

        if (user.IsInRole(UserRole.Customer) && libraryGame?.UserId.Value != user.DomainUserId)
            return Result.Failure<FullLibraryGameResponse>(
                Errors.Libraries.GetLibraryGameById.UserNotAuthorized(request.LibraryGameId));

        return Result.Success(new FullLibraryGameResponse(
            libraryGame.Id.Value,
            libraryGame.GameId.Value,
            libraryGame.PurchaseDate,
            libraryGame.LicenceKey.Value,
            libraryGame.Game.ToResponse()));
    }
}