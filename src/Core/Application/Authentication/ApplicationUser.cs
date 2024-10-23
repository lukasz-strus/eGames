using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication;

public class ApplicationUser : IdentityUser
{
    public UserId? UserId { get; set; }
    public User? User { get; set; }
}