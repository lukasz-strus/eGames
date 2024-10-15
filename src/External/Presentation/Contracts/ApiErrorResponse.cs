using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Contracts;

public class ApiErrorResponse(IEnumerable<Error> errors)
{
    public IEnumerable<Error> Errors { get; } = errors;
}
