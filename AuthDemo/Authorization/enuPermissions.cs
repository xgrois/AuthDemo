namespace AuthDemo.Authorization
{
    [Flags]
    public enum enuPermissions
    {
        None = 0,
        ViewRoles = 1,
        ManageRoles = 2,
        ViewUsers = 4,
        ManageUsers = 8,
        All = int.MaxValue
    }
}
