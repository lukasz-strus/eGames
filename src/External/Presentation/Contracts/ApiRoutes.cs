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

        public const string GetDlcGames = "games/{fullGameId:guid}/dlc";
        public const string GetDlcGame = "games/dlc/{id:guid}";
        public const string CreateDlcGame = "games/{fullGameId:guid}/dlc";
        public const string UpdateDlcGame = "games/dlc/{id:guid}";

        public const string GetSubscriptions = "games/subscriptions";
        public const string GetSubscription = "games/subscriptions/{id:guid}";
        public const string CreateSubscription = "games/subscriptions";
        public const string UpdateSubscription = "games/subscriptions/{id:guid}";
    }
}