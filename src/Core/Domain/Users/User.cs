using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Core.Primitives;

namespace Domain.Users;

public sealed class User : Entity<UserId>
{
    private readonly List<UserRole> _roles = [];

    [Required] [StringLength(100)] public string UserName { get; private set; } = default!;

    [NotMapped] public IReadOnlyCollection<UserRole> Roles => _roles;

    public static User Create(string userName, IEnumerable<UserRole>? roles = null)
    {
        var user = new User
        {
            Id = new UserId(Guid.NewGuid()),
            UserName = userName,
        };

        user.InitializeRoles(roles);

        return user;
    }

    private void InitializeRoles(IEnumerable<UserRole>? roles)
    {
        if (roles is null) return;

        _roles.AddRange(roles.Distinct());
    }

    public void AddRole(UserRole role)
    {
        if (_roles.Contains(role)) return;

        _roles.Add(role);
    }

    public void RemoveRole(UserRole role)
    {
        if (!_roles.Contains(role)) return;

        _roles.Remove(role);
    }
}