using System.ComponentModel.DataAnnotations;

namespace Domain.Users;

public abstract class User
{
    [Key]
    public UserId Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Login { get; private set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; private set; } = string.Empty;

    [Required]
    [Phone]
    [MaxLength(20)]
    public string PhoneNumber { get; private set; } = string.Empty;

}