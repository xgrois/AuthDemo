namespace AuthCustom.Authorization.Definitions
{
    /// <summary>
    /// Todos los permisos de la app deben estar en BBDD y aquí
    /// </summary>
    public static class Permisos
    {
        public static class Pages
        {
            public const string All = "Permisos.Pages.All";
        }

        public static class UsuariosPage
        {
            public const string All = "Permisos.UsuariosPage.All";
            public const string Create = "Permisos.UsuariosPage.Create";
            public const string Read = "Permisos.UsuariosPage.Read";
            public const string Update = "Permisos.UsuariosPage.Update";
            public const string Delete = "Permisos.UsuariosPage.Delete";
        }

        public static class RolesPage
        {
            public const string All = "Permisos.RolesPage.All";
            public const string Create = "Permisos.RolesPage.Create";
            public const string Read = "Permisos.RolesPage.Read";
            public const string Update = "Permisos.RolesPage.Update";
            public const string Delete = "Permisos.RolesPage.Delete";
        }

        public static class PermisosPage
        {
            public const string All = "Permisos.PermisosPage.All";
            public const string Create = "Permisos.PermisosPage.Create";
            public const string Read = "Permisos.PermisosPage.Read";
            public const string Update = "Permisos.PermisosPage.Update";
            public const string Delete = "Permisos.PermisosPage.Delete";
        }

        public static class PrivacyPage
        {
            public const string All = "Permisos.PrivacyPage.All";
        }

    }
}
