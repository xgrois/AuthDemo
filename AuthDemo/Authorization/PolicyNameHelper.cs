namespace AuthDemo.Authorization;

public static class PolicyNameHelper
{
    public const string Prefix = "Permissions";

    public static bool IsValidPolicyName(string? policyName)
    {
        return policyName != null && policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase);
    }

    public static string GeneratePolicyNameFor(enuPermissions permissions)
    {
        return $"{Prefix}{(int)permissions}";
    }

    public static enuPermissions GetPermissionsFrom(string policyName)
    {
        var permissionsValue = int.Parse(policyName[Prefix.Length..]!);

        return (enuPermissions)permissionsValue;
    }
}
