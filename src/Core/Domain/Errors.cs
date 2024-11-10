using Domain.Core.Primitives;

namespace Domain;

public static class Errors
{
    public static class Users
    {
        public static class GetUserById
        {
            public static Error UserNotFound(Guid userId) =>
                new("Users.GetUserById.UserNotFound",
                    $"User with ID {userId} was not found.");
        }

        public static class AddRole
        {
            public static Error RoleNotFound(int roleId) =>
                new("Users.AddRole.RoleNotFound",
                    $"Role with ID {roleId} was not found.");
        }
    }

    public static class Orders
    {
        public static class CreateOrder
        {
            public static Error CustomerNotFound(Guid customerId) =>
                new("Orders.CreateOrder.CustomerNotFound",
                    $"Customer with ID {customerId} was not found.");
        }

        public static class GetOrderById
        {
            public static Error OrderNotFound(Guid orderId) =>
                new("Orders.GetOrderById.OrderNotFound",
                    $"Order with ID {orderId} was not found.");
        }

        public static class GetOrderItemById
        {
            public static Error OrderItemNotFound(Guid orderItemId) =>
                new("Orders.GetOrderItemById.OrderItemNotFound",
                    $"Order item with ID {orderItemId} was not found.");
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

    public static class Libraries
    {
        public static class GetLibraryGameById
        {
            public static Error LibraryGameNotFound(Guid libraryId) =>
                new("Libraries.GetLibraryById.LibraryNotFound",
                    $"Library with ID {libraryId} was not found.");

            public static Error UserNotAuthorized(Guid requestLibraryGameId) =>
                new("Libraries.GetLibraryById.UserNotAuthorized",
                    $"User is not authorized to access library game with ID {requestLibraryGameId}.");
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