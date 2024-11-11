using Domain;
using Domain.Core.Primitives;

namespace Presentation.Contracts;

public class ApiErrorResponse(IEnumerable<Error> errors)
{
    public IEnumerable<Error> Errors { get; } = errors;
}