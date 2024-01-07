using Microsoft.AspNetCore.Components;

namespace AuthCustom.Components
{
    public class FlexibleAuthorizeView : Microsoft.AspNetCore.Components.Authorization.AuthorizeView
    {
        [Parameter]
        public string? Permisos
        {
            get
            {
                //return string.IsNullOrEmpty(Policy) ? Permissions.None : PolicyNameHelper.GetPermissionsFrom(Policy);
                return Policy;
            }
            set
            {
                Policy = value;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}
