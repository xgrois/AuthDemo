using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AuthCustom.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public LogoutModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var usernameClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (usernameClaim != null)
            {
                _logger.LogInformation($"Sign out of user {usernameClaim.Value}");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }

    }
}
