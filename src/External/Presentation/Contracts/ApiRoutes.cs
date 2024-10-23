namespace Presentation.Contracts;

public static class ApiRoutes
{
    public static class Games
    {
        public const string GetGames = "games";
        public const string GetGame = "games/{id:guid}";
    }

    public static class Identity
    {
        public const string Register = "identity/register2";
        public const string Login = "identity/login";
    }
}