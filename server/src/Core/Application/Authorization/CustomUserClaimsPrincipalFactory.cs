using Application.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Application.Authorization;

public class CustomUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var id = await GenerateClaimsAsync(user);

        if (user.UserId is not null)
        {
            id.AddClaim(new Claim(AppClaimTypes.DomainUserId, user.UserId.Value.ToString()));
        }

        return new ClaimsPrincipal(id);
    }
}