using Domain.Core.Primitives;

namespace Domain.Users;

public sealed class UserRole : Enumeration<UserRole>
{
    public static readonly UserRole Customer = new(1, UserRoleNames.Customer);
    public static readonly UserRole Admin = new(2, UserRoleNames.Admin);
    public static readonly UserRole SuperAdmin = new(3, UserRoleNames.SuperAdmin);

    // ReSharper disable once UnusedMember.Local
    private UserRole()
    {
    }

    private UserRole(int value, string name)
        : base(value, name)
    {
    }
}

public static class UserRoleNames
{
    public const string Customer = "Customer";
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
}