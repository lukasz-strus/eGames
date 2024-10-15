﻿namespace Domain.Core.Primitives;

public record Error
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
 
    public string Message { get; }

    public static implicit operator string(Error? error) => error?.Code ?? string.Empty;

    internal static Error None => new Error(string.Empty, string.Empty);
}
