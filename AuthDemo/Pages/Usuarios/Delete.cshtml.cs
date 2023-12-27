using AuthDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemo.Pages.Usuarios
{
    public class DeleteModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUserStore<Usuario> _userStore;
        //private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<EditModel> _logger;
        //private readonly IEmailSender _emailSender;

        public DeleteModel(
            UserManager<Usuario> userManager,
            IUserStore<Usuario> userStore,
            SignInManager<Usuario> signInManager,
            ILogger<EditModel> logger
            //IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id.ToString()!);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }
            else
            {
                Usuario = usuario;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id.ToString()!);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            var result = await _userManager.DeleteAsync(usuario);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
