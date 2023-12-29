using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Authorization;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public PermissionAuthorizationRequirement(enuPermissions permission)
    {
        Permissions = permission;
    }

    public enuPermissions Permissions { get; }
}