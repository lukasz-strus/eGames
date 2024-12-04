using Application.Contracts.Common;

namespace Application.Contracts.Libraries;

public sealed class LibraryGameListResponse(
    IReadOnlyCollection<LibraryGameResponse> items,
    int totalCount,
    int? pageSize,
    int? pageNumber) : PagedResult(totalCount, pageSize, pageNumber)
{
    public IReadOnlyCollection<LibraryGameResponse> Items { get; } = items;
}