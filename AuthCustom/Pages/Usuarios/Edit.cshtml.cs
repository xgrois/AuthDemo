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
    public class EditModel : PageModel
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
        public EditModel(AuthCustom.Data.AuthCustomContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }
        #endregion

        #region ViewModels
        public class InputModel
        {
            public int IdUsuarios { get; set; }

            [Required]
            [Display(Name = "Nombre de usuario")]
            public string UserName { get; set; } = string.Empty;

            public List<RolVM> RolesVM { get; set; } = new();

            public class RolVM
            {
                public int IdRoles { get; set; }
                public string Nombre { get; set; } = string.Empty;
                public bool Asignado { get; set; }
            }
        }

        #endregion

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // No debemos bindear directamente un obj de BD
            // Si se hace así, cualquiera con HTML/javascript podría "asignar" campos del obj que no mostramos en la UI (overposting attack)
            // https://andrewlock.net/preventing-mass-assignment-or-over-posting-with-razor-pages-in-asp-net-core/
            var usuario = await _context.Usuarios.AsNoTracking().Include(x => x.UsuariosRoles).ThenInclude(x => x.Rol).FirstOrDefaultAsync(m => m.IdUsuarios == id);
            if (usuario == null)
            {
                return NotFound();
            }

            Input.IdUsuarios = usuario.IdUsuarios;
            Input.UserName = usuario.UserName;

            var roles = await _context.Roles.ToListAsync();

            if (roles.Any())
            {
                foreach (var rol in roles)
                {
                    var rolVM = new InputModel.RolVM() { IdRoles = rol.IdRoles, Nombre = rol.Nombre, Asignado = false };
                    if (usuario.UsuariosRoles.Find(x => x.IdRoles == rol.IdRoles) != null)
                    {
                        rolVM.Asignado = true;
                    }
                    Input.RolesVM.Add(rolVM);
                }
            }

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

            var usuario = await _context.Usuarios.Include(x => x.UsuariosRoles).FirstOrDefaultAsync(m => m.IdUsuarios == Input.IdUsuarios);
            if (usuario == null)
            {
                return NotFound();
            }

            // Aunque ya debería estar trackeando al hacer la consulta anterior,
            // si no añades explícitamente ésto, el UserName no lo actualiza (aunque sea != del que ya tiene)
            // Sí actualiza los roles del usuario pero no el UserName...no entiendo
            _context.Attach(usuario).State = EntityState.Modified;

            usuario.UserName = Input.UserName;

            if (usuario.UsuariosRoles.Any())
            {
                _context.UsuariosRoles.RemoveRange(usuario.UsuariosRoles);
            }

            var usuarioRoles = new List<UsuarioRol>();
            foreach (var rolVM in Input.RolesVM)
            {
                if (rolVM.Asignado == true)
                {
                    usuarioRoles.Add(new UsuarioRol { IdUsuarios = Input.IdUsuarios, IdRoles = rolVM.IdRoles });
                }
            }

            usuario.UsuariosRoles = usuarioRoles;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(Input.IdUsuarios))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            StatusMessage = "La edición se ha guardado correctamente";

            // Esto es importante, después de hacer un POST donde guardamos los datos que hemos cubierto,
            // debemos usar RedirectToPage() y no Page() para prevenir que si se refresca el navegador vuelva a hacer el POST
            // Se debe usar el patrón POST - REDIRECT - GET
            return RedirectToPage("/Usuarios/Edit", new { id = usuario.IdUsuarios });
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuarios == id);
        }

    }
}
