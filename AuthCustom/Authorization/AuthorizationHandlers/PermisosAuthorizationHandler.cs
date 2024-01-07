using AuthCustom.Authorization.Definitions;
using AuthCustom.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthCustom.Authorization.AuthorizationHandlers
{
    public class PermisosAuthorizationHandler : AuthorizationHandler<PermisosRequirement>
    {
        private readonly ILogger<PermisosAuthorizationHandler> _logger;

        public PermisosAuthorizationHandler(ILogger<PermisosAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisosRequirement requirement)
        {
            // If the user is not authenticated first, do nothing and return
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            // If the user was already authorized, do nothing and return
            if (context.HasSucceeded)
            {
                return Task.CompletedTask;
            }

            // If the user has super powers, authorize and return
            if (context.User.IsInRole(Roles.Admin))
            {
                AuthHandlerContextHelper.SucceedAll(context, requirement.Id);
                return Task.CompletedTask;
            }

            // If the user does not exist or the requirements are not properly set, do nothing and return
            if (context.User == null || requirement == null || string.IsNullOrWhiteSpace(requirement.Permisos))
            {
                return Task.CompletedTask;
            }

            // Get the requirements based on the format "R1|R2|R3|RN" and mandate having at least 1 of them
            var requirementTokens = requirement.Permisos.Split("|", StringSplitOptions.RemoveEmptyEntries);

            // If no requirements, do nothing and return
            if (requirementTokens?.Any() != true)
            {
                return Task.CompletedTask;
            }

            // Fetch user permission claims
            var userPermissionClaims = context.User.Claims?.Where(c =>
                string.Equals(c.Type, CustomClaimTypes.CustomClaimTypes.Permiso, StringComparison.OrdinalIgnoreCase));


            foreach (var claim in userPermissionClaims ?? Enumerable.Empty<Claim>())
            {
                if (IsClaimInRequiredPermissions(claim, requirementTokens))
                {
                    // Succeed all requirements in the context having the same identifier
                    AuthHandlerContextHelper.SucceedAll(context, requirement.Id);
                    break;
                }
            }

            return Task.CompletedTask;
        }

        private bool IsClaimInRequiredPermissions(Claim claim, string[] requirementTokens)
        {
            foreach (var token in requirementTokens)
            {
                if (claim.Type == CustomClaimTypes.CustomClaimTypes.Permiso && claim.Value == token)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
