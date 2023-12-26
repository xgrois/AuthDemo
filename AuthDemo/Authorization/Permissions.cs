namespace AuthDemo.AccessControl
{
    public static class Permissions
    {
        public static class PrivacyPage
        {
            public const string View = "Permissions.PrivacyPage.View";
        }
        public static class UsersPage
        {
            public const string View = "Permissions.UsersPage.View";
            public const string Create = "Permissions.UsersPage.Create";
            public const string Edit = "Permissions.UsersPage.Edit";
            public const string Delete = "Permissions.UsersPage.Delete";
        }
    }
}
