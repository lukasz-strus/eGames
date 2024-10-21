namespace Application.Authentication;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}