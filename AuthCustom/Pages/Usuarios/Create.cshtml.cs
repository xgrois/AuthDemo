using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Usuarios
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.Create })]
    public class CreateModel : PageModel
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
        public CreateModel(AuthCustom.Data.AuthCustomContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region InputModels
        public class InputModel
        {
            [Required]
            [Display(Name = "Nombre de usuario")]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            [Display(Name = "Confirmar contraseña")]
            public string ConfirmPassword { get; set; } = string.Empty;

            public List<RolVM> RolesVM { get; set; } = new();

            public class RolVM
            {
                public int IdRoles { get; set; }
                public string Nombre { get; set; } = string.Empty;
                public bool Asignado { get; set; }
            }
        }
        #endregion

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new();
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            var roles = await _context.Roles.ToListAsync();

            foreach (var rol in roles)
            {
                Input.RolesVM.Add(new InputModel.RolVM() { IdRoles = rol.IdRoles, Nombre = rol.Nombre, Asignado = false });
            }

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuarioYaExiste = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == Input.UserName);
            if (usuarioYaExiste != null)
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario ya existe.");
                return Page();
            }

            Usuario usuario = BuildUsuario(Input.UserName, Input.Password, Input.RolesVM);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        #region Helpers
        private Usuario BuildUsuario(string pUserName, string pPassword, List<InputModel.RolVM> pRolesVM)
        {
            Usuario usuario = new Usuario() { UserName = pUserName, Password = "" };

            string passwordHashed = _passwordHasher.HashPassword(usuario, pPassword);

            usuario.Password = passwordHashed;

            if (pRolesVM != null && pRolesVM.Count > 0)
            {
                usuario.UsuariosRoles = new List<UsuarioRol>();
                foreach (var rolVM in pRolesVM)
                {
                    if (rolVM.Asignado == true)
                    {
                        usuario.UsuariosRoles.Add(new UsuarioRol()
                        {
                            IdRoles = rolVM.IdRoles,
                            IdUsuarios = usuario.IdUsuarios,
                        });
                    }
                }
            }

            return usuario;
        }
        #endregion


    }
}
