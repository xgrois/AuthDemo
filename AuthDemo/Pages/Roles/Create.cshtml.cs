using AuthDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemo.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly RoleManager<Rol> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUserStore<Usuario> _userStore;
        //private readonly IUserEmailStore<Usuario> _emailStore;
        private readonly ILogger<CreateModel> _logger;
        //private readonly IEmailSender _emailSender;

        public CreateModel(
            RoleManager<Rol> roleManager,
            UserManager<Usuario> userManager,
            IUserStore<Usuario> userStore,
            SignInManager<Usuario> signInManager,
            ILogger<CreateModel> logger
            //IEmailSender emailSender
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Rol Rol { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Rol newRol = new Rol();
            newRol.Name = Rol.Name;

            if (await _roleManager.RoleExistsAsync(newRol.Name))
            {
                ModelState.AddModelError(string.Empty, $"Rol {newRol.Name} already exists.");
                return Page();
            }

            var result = await _roleManager.CreateAsync(newRol);

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
