using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Usuarios
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.Update })]
    public class ChangePasswordModel : PageModel
    {
        #region Consts

        #endregion

        #region Fields
        private readonly AuthCustom.Data.AuthCustomContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ChangePasswordModel(AuthCustom.Data.AuthCustomContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [TempData]
        public string? StatusMessage { get; set; }
        #endregion

        #region InputModels
        public class InputModel
        {
            public int IdUsuarios { get; set; }

            [Required]
            [Display(Name = "Nombre de usuario")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            [Display(Name = "Confirmar nueva contraseña")]
            public string ConfirmPassword { get; set; }
        }
        #endregion

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.IdUsuarios == id);
            if (usuario == null)
            {
                return NotFound();
            }

            Input.IdUsuarios = id;
            Input.UserName = usuario.UserName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.UserName == Input.UserName);
            if (usuario == null)
            {
                return NotFound();
            }

            string password = Input.Password;

            await SaveUsuarioAsync(usuario, password);

            StatusMessage = "Contraseña cambiada";

            // Esto es importante, después de hacer un POST donde guardamos los datos que hemos cubierto,
            // debemos usar RedirectToPage() y no Page() para prevenir que si se refresca el navegador vuelva a hacer el POST
            // Se debe usar el patrón POST - REDIRECT - GET
            return RedirectToPage("/Usuarios/ChangePassword", new { id = usuario.IdUsuarios });

            //return LocalRedirect(returnUrl);
        }

        #region Helpers
        private async Task SaveUsuarioAsync(Usuario usuario, string password)
        {
            string passwordHashed = _passwordHasher.HashPassword(usuario, password);

            usuario.Password = passwordHashed;

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
