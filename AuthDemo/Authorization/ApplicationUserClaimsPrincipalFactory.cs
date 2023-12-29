using AuthDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Data;
using System.Security.Claims;

namespace AuthDemo.Authorization;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<Usuario, Rol>
{
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<Usuario> userManager,
        RoleManager<Rol> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
    : base(userManager, roleManager, optionsAccessor)
    { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var userRoleNames = await UserManager.GetRolesAsync(user) ?? Array.Empty<string>();

        var userRoles = await RoleManager.Roles.Where(r =>
            userRoleNames.Contains(r.Name ?? string.Empty)).ToListAsync();

        var userPermissions = enuPermissions.None;

        foreach (var role in userRoles)
            userPermissions |= role.Permissions;

        var permissionsValue = (int)userPermissions;

        identity.AddClaim(
            new Claim(CustomClaimTypes.Permissions, permissionsValue.ToString()));

        return identity;
    }
}
