namespace Domain.Primitives;

public class DomainException(string message) : Exception(message)
{
}
