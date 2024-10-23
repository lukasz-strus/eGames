using System.ComponentModel.DataAnnotations;
using Domain.Core.Primitives;

namespace Domain.Users;

public abstract class User : Entity<UserId>
{
    [Required] [StringLength(100)] public string UserName { get; protected set; } = default!;
}