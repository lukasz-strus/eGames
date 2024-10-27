namespace Presentation.Contracts;

public static class ApiRoutes
{
    public static class Games
    {
        public const string GetGames = "games";

        public const string GetFullGames = "games/full";
        public const string GetFullGame = "games/full/{id:guid}";
        public const string CreateFullGame = "games/full";

        public const string GetDlcGames = "games/{fullGameId:guid}/dlc";
        public const string GetDlcGame = "games/{fullGameId:guid}/dlc/{id:guid}";
        public const string CreateDlcGame = "games/{fullGameId:guid}/dlc";

        public const string GetSubscriptions = "games/subscription";
        public const string GetSubscription = "games/subscription/{id:guid}";
        public const string CreateSubscription = "games/subscription";
    }
}