using Domain.Core.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users;

public abstract class User : Entity<UserId>
{
    [Required] [MaxLength(100)] public string Login { get; protected set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; protected set; } = string.Empty;

    [Required] [MaxLength(100)] public string FirstName { get; protected set; } = string.Empty;

    [Required] [MaxLength(100)] public string LastName { get; protected set; } = string.Empty;

    [Required] [Phone] [MaxLength(20)] public string PhoneNumber { get; protected set; } = string.Empty;
}