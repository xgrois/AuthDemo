using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AuthCustom.Authorization.AuthorizationPolicyProviders
{
    public class MyAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public MyAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// This is called when no policy is specified which is null by default.
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        /// <summary>
        /// This is called when we use [MyAuthorize] attribute only, without any parameter
        /// We require the User to be authenticated if we only use the [MyAuthorize]
        /// </summary>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
        }

        /// <summary>
        /// This is called when we use [MyAuthorize] attribute with parameters, e.g. [MyAuthorize(Permisos = new[] { "Read", "Write" }, Roles = new[] { "Admin" })]
        /// which is going to generate a string "Permisos$Read|Write;Roles$Admin;" for the policyName
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName))
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            var policyTokens = policyName.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (policyTokens?.Any() != true)
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
            var identifier = Guid.NewGuid();

            foreach (var token in policyTokens)
            {
                var pair = token.Split('$', StringSplitOptions.RemoveEmptyEntries);

                if (pair?.Any() != true || pair.Length != 2)
                {
                    return FallbackPolicyProvider.GetPolicyAsync(policyName);
                }

                IAuthorizationRequirement requirement = (pair[0]) switch
                {
                    MyAuthorizeAttribute.TagPermisos => new PermisosRequirement(pair[1], identifier),
                    MyAuthorizeAttribute.TagRoles => new RolesRequirement(pair[1], identifier),
                    _ => null,
                };

                if (requirement == null)
                {
                    return FallbackPolicyProvider.GetPolicyAsync(policyName);
                }

                policy.AddRequirements(requirement);
            }

            return Task.FromResult(policy.Build());
        }
    }
}
