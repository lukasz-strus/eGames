using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Libraries;
using MediatR;

namespace Application.Libraries.Delete;

internal sealed class DeleteLibraryGameCommandHandler(
    ILibraryRepository libraryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteLibraryGameCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteLibraryGameCommand request, CancellationToken cancellationToken)
    {
        var game = await libraryRepository.GetByIdAsync(new LibraryGameId(request.LibraryGameId), cancellationToken);

        if (game is null)
            return Result.Failure<Unit>(Errors.Games.GetGameById.GameNotFound(request.LibraryGameId));

        libraryRepository.Delete(game);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}