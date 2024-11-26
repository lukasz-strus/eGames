using Domain.Core.Exceptions;

namespace Infrastructure.Exceptions;

public sealed class DatabaseException(string message) : DomainException(message);