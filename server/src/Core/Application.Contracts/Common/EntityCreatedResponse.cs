namespace Application.Contracts.Common;

public sealed class EntityCreatedResponse(Guid id)
{
    public Guid Id { get; } = id;
}