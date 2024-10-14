using Domain.Primitives;

namespace Domain.Users;

public class UserNotFoundException(string message) : DomainException(message)
{
}
