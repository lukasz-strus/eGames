﻿namespace Presentation.Contracts;

public static class ApiRoutes
{
    public static class Games
    {
        public const string GetGames = "games";
        public const string GetGame = "games/{id:guid}";
    }

    public static class Users
    {
        public const string Login = "login";
    }
}