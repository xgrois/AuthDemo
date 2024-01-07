using Microsoft.AspNetCore.Authorization;

namespace AuthCustom.Authorization.Requirements
{
    public class PermisosRequirement : IAuthorizationRequirement, IIdentificableRequirement
    {
        public Guid Id { get; set; }
        public string Permisos { get; }

        public PermisosRequirement(string permisos, Guid identificador)
        {
            Permisos = permisos;
            Id = identificador;
        }
    }
}
