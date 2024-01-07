using Microsoft.AspNetCore.Authorization;

namespace AuthCustom.Authorization.Requirements
{
    public class RolesRequirement : IAuthorizationRequirement, IIdentificableRequirement
    {
        public Guid Id { get; set; }
        public string Roles { get; }

        public RolesRequirement(string roles, Guid identificador)
        {
            Roles = roles;
            Id = identificador;
        }
    }
}
