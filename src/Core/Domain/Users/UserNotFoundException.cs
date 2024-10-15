using Domain.Core.Exceptions;

namespace Domain.Users;

public class UserNotFoundException(string message) : DomainException(message)
{
}
