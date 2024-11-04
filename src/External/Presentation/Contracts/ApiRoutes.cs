namespace Presentation.Contracts;

public static class ApiRoutes
{
    public static class Games
    {
        public const string GetGames = "games";
        public const string PublishGame = "games/{id:guid}/publish";
        public const string UnpublishGame = "games/{id:guid}/unpublish";
        public const string DeleteGame = "games/{id:guid}";
        public const string RestoreGame = "games/{id:guid}/restore";

        public const string GetFullGames = "games/full";
        public const string GetFullGame = "games/full/{id:guid}";
        public const string CreateFullGame = "games/full";
        public const string UpdateFullGame = "games/full/{id:guid}";

        public const string GetDlcGames = "games/full/{id:guid}/dlc";
        public const string GetDlcGame = "games/dlc/{id:guid}";
        public const string CreateDlcGame = "games/full/{id:guid}/dlc";
        public const string UpdateDlcGame = "games/dlc/{id:guid}";

        public const string GetSubscriptions = "games/subscriptions";
        public const string GetSubscription = "games/subscriptions/{id:guid}";
        public const string CreateSubscription = "games/subscriptions";
        public const string UpdateSubscription = "games/subscriptions/{id:guid}";
    }


    public static class Orders
    {
        public const string CreateOrder = "orders";
        public const string GetById = "orders/{id:guid}";
        public const string GetOrderItems = "orders/{id:guid}/items";
        public const string GetOrderItem = "orders/items/{id:guid}";
        public const string CreateOrderItem = "orders/{id:guid}/items";
        public const string RemoveOrderItem = "orders/{id:guid}/items/{itemId:guid}";
        public const string PayOrder = "orders/{id:guid}/pay";
        public const string DeleteOrder = "orders/{id:guid}";

        public static class Users
        {
            public const string GetOwnOrders = "me/orders";
            public const string GetUserOrders = "users/{id:guid}/orders";
        }
    }
}