using AuthCustom.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AuthCustom.Authorization.AuthorizationHandlers
{
    public static class AuthHandlerContextHelper
    {
        /// <summary>
        /// Sets all requirements with the same Id to Succeed
        /// This is useful when we are evaluating an OR condition of multiple requirements in a Policy
        /// </summary>
        /// <param name="context"></param>
        /// <param name="identifier"></param>
        public static void SucceedAll(AuthorizationHandlerContext context, Guid identifier)
        {
            var tagRequirements = context.Requirements.Where(r => (r as IIdentificableRequirement)?.Id == identifier);

            foreach (var requirement in tagRequirements)
            {
                context.Succeed(requirement);
            }
        }
    }
}
