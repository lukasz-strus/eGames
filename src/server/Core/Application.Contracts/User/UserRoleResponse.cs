namespace Application.Contracts.User;

public sealed class UserRoleResponse(
    int id,
    string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}