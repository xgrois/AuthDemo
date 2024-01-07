using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCustom.Pages
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.PrivacyPage.All })]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
