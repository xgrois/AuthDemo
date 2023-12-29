namespace AuthDemo.Authorization;

public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeAttribute() { }

    public AuthorizeAttribute(string policy) : base(policy) { }

    public AuthorizeAttribute(enuPermissions permission)
    {
        Permissions = permission;
    }

    public enuPermissions Permissions
    {
        get
        {
            return !string.IsNullOrEmpty(Policy)
                ? PolicyNameHelper.GetPermissionsFrom(Policy)
                : enuPermissions.None;
        }
        set
        {
            Policy = value != enuPermissions.None
                ? PolicyNameHelper.GeneratePolicyNameFor(value)
                : string.Empty;
        }
    }
}