namespace Presentation.Contracts;

public static class ApiRoutes
{
    public static class Games
    {
        public const string GetGames = "games";

        public const string GetFullGame = "games/{id:guid}";
        public const string CreateFullGame = "games";

        public const string GetDlcGames = "games/{baseGameId:guid}/dlc";
        public const string GetDlcGame = "games/dlc/{id:guid}";
        public const string CreateDlcGame = "games/{baseGameId:guid}/dlc";

        public const string GetSubscription = "games/subscription/{id:guid}";
    }
}