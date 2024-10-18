using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public UserId? UserId { get; set; }
    public User? User { get; set; }
}