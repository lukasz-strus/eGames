using System.Security.Claims;
using Application.Authorization;
using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Authentication;

internal sealed class UserContext(
    IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user is null)
            throw new InvalidOperationException("Context user is not present");

        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        var userId = user.FindFirst(AppClaimTypes.DomainUserId)?.Value;
        var roles = user.FindAll(ClaimTypes.Role).Select(c => UserRole.FromName(c.Value)!);

        if (id is null || email is null || userId is null || roles is null)
            throw new InvalidOperationException("User claims are not present");

        return new CurrentUser(Guid.Parse(id), email, Guid.Parse(userId), roles);
    }
}