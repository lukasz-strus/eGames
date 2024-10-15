namespace Domain.Core.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, params object?[] keys)
        : base(string.Format(message, keys))
    {
    }
}