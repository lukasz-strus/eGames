namespace Application.Contracts.User;

public sealed class UserListResponse(IReadOnlyCollection<UserResponse> items)
{
    public IReadOnlyCollection<UserResponse> Items { get; } = items;
}