using AuthDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemo.Pages.Roles
{
    public class EditModel : PageModel
    {
        private readonly RoleManager<Rol> _roleManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUserStore<Usuario> _userStore;
        //private readonly IUserEmailStore<Usuario> _emailStore;
        private readonly ILogger<CreateModel> _logger;
        //private readonly IEmailSender _emailSender;

        public EditModel(
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

        [BindProperty]
        public Rol Rol { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _roleManager.FindByIdAsync(id.ToString()!);

            if (rol == null)
            {
                return NotFound($"Rol ID {id} not found.");
            }
            Rol = rol;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var rol = await _roleManager.FindByIdAsync(Rol.Id.ToString());

            if (rol == null)
            {
                return NotFound($"Rol ID {Rol.Id} not found.");
            }

            if (!string.IsNullOrWhiteSpace(Rol.Name) && !Rol.Name.Equals(rol.Name))
            {
                rol.Name = Rol.Name;
                var result = await _roleManager.UpdateAsync(rol);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
