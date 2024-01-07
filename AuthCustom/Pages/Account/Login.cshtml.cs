using AuthCustom.Authorization.CustomClaimTypes;
using AuthCustom.Data;
using AuthCustom.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthCustom.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AuthCustomContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(AuthCustomContext context, IPasswordHasher<Usuario> passwordHasher, ILogger<LoginModel> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [BindProperty]
        public required InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios.AsNoTracking()
                    .Include(x => x.UsuariosRoles)
                    .ThenInclude(x => x.Rol)
                    .ThenInclude(x => x.RolesPermisos)
                    .ThenInclude(x => x.Permiso)
                    .FirstOrDefaultAsync(x => x.UserName == Input.UserName);

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to log in.");
                    return Page();
                }

                // Check user/pass
                var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, Input.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "Failed to log in.");
                    return Page();
                }

                ClaimsPrincipal claimsPrincipal = BuildUserClaimsPrincipal(usuario);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                _logger.LogInformation($"UserName {usuario.UserName} logged in at {DateTime.UtcNow}.");

                return LocalRedirect(returnUrl);

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private static ClaimsPrincipal BuildUserClaimsPrincipal(Usuario usuario)
        {
            // Basic identification claims (existing Usuario will always have)
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuarios.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
            };

            // Add user roles as "role-claims"
            // Aunque los roles se cargan de una tabla en BBDD (separada de permisos), a la hora de crear el ClaimsPrincipal del HttpContext.User
            // se meten como claims con el tipo p.e. "rol"
            // Luego, cuando se crea el ClaimsIdentity, se indica que los claims de tipo "rol" se deben tratar como roles del usuario
            // Eso significa que luego podríamos usar funciones como HttpContext.User.IsInRole("cliente") que ya tiene ASP .NET Core
            if (usuario.UsuariosRoles.Any())
            {
                foreach (var ur in usuario.UsuariosRoles)
                {
                    userClaims.Add(new Claim(CustomClaimTypes.Rol, ur.Rol.Nombre));
                }
            }

            // Add rol permissions as "permission-claims"
            if (usuario.UsuariosRoles.Any())
            {
                foreach (var ur in usuario.UsuariosRoles)
                {
                    if (ur.Rol.RolesPermisos.Any())
                    {
                        foreach (var rp in ur.Rol.RolesPermisos)
                        {
                            userClaims.Add(new Claim(CustomClaimTypes.Permiso, rp.Permiso.Nombre));
                        }
                    }
                }
            }

            var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme, null, roleType: CustomClaimTypes.Rol);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }

        public class InputModel
        {
            [Required]
            public required string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public required string Password { get; set; }

        }

    }
}
