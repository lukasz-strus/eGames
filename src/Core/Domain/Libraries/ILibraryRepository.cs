using Domain.Users;

namespace Domain.Libraries;

public interface ILibraryRepository
{
    Task AddAsync(LibraryGame libraryGame, CancellationToken cancellationToken);
    Task<List<LibraryGame>> GetAllByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<LibraryGame?> GetByIdAsync(LibraryGameId id, CancellationToken cancellationToken);
    void Delete(LibraryGame game);
}