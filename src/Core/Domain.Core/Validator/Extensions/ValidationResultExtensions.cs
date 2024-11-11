using Domain.Core.Primitives;

namespace Domain.Core.Validator.Extensions;

public static class ValidationResultExtensions
{
    public static async Task<T> Match<TValue, T>(
        this Task<ValidationResult<TValue>> resultTask,
        Func<TValue, T> onSuccess,
        Func<Error[], T> onFailure)
    {
        var result = await resultTask;

        return result.Match(onSuccess, onFailure);
    }

    public static T Match<TValue, T>(this ValidationResult<TValue> result, Func<TValue, T> onSuccess,
        Func<Error[], T> onFailure) =>
        result.IsSuccess ? onSuccess(result.Value()) : onFailure(result.Errors);
}