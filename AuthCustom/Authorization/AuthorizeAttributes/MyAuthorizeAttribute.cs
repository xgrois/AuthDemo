using Microsoft.AspNetCore.Authorization;

namespace AuthCustom.Authorization.AuthorizeAttributes
{
    /// <summary>
    /// Permite usar atributos del tipo [MyAuthorize(Permisos = new[] { "Read", "Write" }, Roles = new[] { "Admin" })]
    /// Internamente setea la propiedad heredada "Policy" a un string con el siguiente formato (siguiendo el ejemplo):
    /// Policy = "Permisos$Read|Write;Roles$Admin;"
    /// Se podría modificar para añadir otros tags si hubiese interés
    /// </summary>
    public sealed class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public const string TagPermisos = CustomAuthorizeTags.Permisos;
        public const string TagRoles = CustomAuthorizeTags.Roles;

        private string[] _permisos;
        private string[] _roles;

        private bool _isDefault = true;

        public MyAuthorizeAttribute()
        {
            _permisos = Array.Empty<string>();
            _roles = Array.Empty<string>();
        }

        public string[] Permisos
        {
            get => _permisos;
            set
            {
                BuildPolicy(ref _permisos, value, TagPermisos);
            }
        }

        new public string[] Roles
        {
            get => _roles;
            set
            {
                BuildPolicy(ref _roles, value, TagRoles);
            }
        }

        private void BuildPolicy(ref string[] valoresGuardados, string[] valoresRecibidos, string tag)
        {
            valoresGuardados = valoresRecibidos ?? Array.Empty<string>();

            if (_isDefault)
            {
                Policy = string.Empty;
                _isDefault = false;
            }

            Policy += $"{tag}${string.Join("|", valoresGuardados)};";
        }
    }
}
