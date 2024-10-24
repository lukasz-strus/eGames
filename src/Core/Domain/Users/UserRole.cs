using Domain.Core.Primitives;

namespace Domain.Users;

public sealed class UserRole : Enumeration<UserRole>
{
    public static readonly UserRole Customer = new(1, "Customer");
    public static readonly UserRole Admin = new(2, "Admin");
    public static readonly UserRole SuperAdmin = new(3, "SuperAdmin");

    // ReSharper disable once UnusedMember.Local
    private UserRole()
    {
    }

    private UserRole(int value, string name)
        : base(value, name)
    {
    }
}