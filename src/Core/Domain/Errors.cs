using Domain.Core.Primitives;

namespace Domain;

public static class Errors
{
    public static class Orders
    {
        public static class CreateOrder
        {
            public static Error CustomerNotFound(Guid customerId) =>
                new("Orders.CreateOrder.CustomerNotFound",
                    $"Customer with ID {customerId} was not found.");
        }
    }

    public static class Games
    {
        public static class GetGameById
        {
            public static Error GameNotFound(Guid gameId) =>
                new("Games.GetGameById.GameNotFound",
                    $"Game with ID {gameId} was not found.");
        }
    }

    public static class Currency
    {
        public static class FromValue
        {
            public static Error InvalidCurrencyValue(int value) =>
                new("Currency.FromValue.InvalidCurrencyValue",
                    $"The currency with ID '{value}' was not found.");
        }
    }

    public static class General
    {
        public static Error BadRequest => new(
            "General.BadRequest",
            "The server could not process the request.");

        public static Error EntityNotFound => new(
            "General.EntityNotFound",
            "The entity with the specified identifier was not found.");

        public static Error ServerError => new(
            "General.ServerError",
            "The server encountered an unrecoverable error.");
    }
}