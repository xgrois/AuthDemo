using AuthDemo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthDemo.Pages.Usuarios
{
    public class EditModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IUserStore<Usuario> _userStore;
        //private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<EditModel> _logger;
        //private readonly IEmailSender _emailSender;

        public EditModel(
            UserManager<Usuario> userManager,
            RoleManager<Rol> roleManager,
            IUserStore<Usuario> userStore,
            SignInManager<Usuario> signInManager,
            ILogger<EditModel> logger
            //IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [EmailAddress]
            public string? Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            public string? PhoneNumber { get; set; }
        }

        public class UsuarioRol
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Checked { get; set; } = false;

        }

        [BindProperty]
        public List<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();

        [BindProperty]
        public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByIdAsync(id.ToString()!); //await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            Usuario = usuario;

            Input = new InputModel
            {
                Email = usuario.Email,
                PhoneNumber = usuario.PhoneNumber
            };

            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var rol in roles)
            {
                UsuarioRoles.Add(new UsuarioRol() { Id = rol.Id, Name = rol.Name, Checked = false });
            }

            var usuarioRoles = await _userManager.GetRolesAsync(usuario);

            foreach (var rol in usuarioRoles)
            {
                CheckRol(rol);
            }

            return Page();
        }

        private void CheckRol(string rolUsuario)
        {
            foreach (var rolDB in UsuarioRoles)
            {
                if (rolDB.Name.Equals(rolUsuario))
                {
                    rolDB.Checked = true;
                }
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _userManager.FindByIdAsync(Usuario.Id.ToString());

            if (usuario is null)
            {
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Input.Email) && !Input.Email.Equals(Usuario.Email))
            {
                var result = await _userManager.SetEmailAsync(usuario, Input.Email);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            if (!string.IsNullOrWhiteSpace(Input.PhoneNumber) && !Input.PhoneNumber.Equals(Usuario.PhoneNumber))
            {
                var result = await _userManager.SetPhoneNumberAsync(usuario, Input.PhoneNumber);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            if (UsuarioRoles != null)
            {
                foreach (var rol in UsuarioRoles)
                {

                    if (rol.Checked)
                    {
                        if (!await _userManager.IsInRoleAsync(usuario, rol.Name))
                        {
                            await _userManager.AddToRoleAsync(usuario, rol.Name);
                        }
                    }
                    else
                    {
                        if (await _userManager.IsInRoleAsync(usuario, rol.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(usuario, rol.Name);
                        }
                    }

                }
            }


            //_context.Attach(User).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!UserExists(User.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            // All OK
            //return RedirectToPage("./Index");
            var myAddress = Request.Path.Value + Request.QueryString.Value;
            return new LocalRedirectResult(myAddress);
        }

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
