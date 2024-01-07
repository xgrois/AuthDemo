using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Roles
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.Update })]
    public class EditModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public EditModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }
        #endregion

        #region ViewModels
        public class InputModel
        {
            public int IdRoles { get; set; }

            [Required]
            public string Nombre { get; set; } = string.Empty;

            public List<PermisoVM> PermisosVM { get; set; } = new();

            public class PermisoVM
            {
                public int IdPermisos { get; set; }
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
            var rol = await _context.Roles.AsNoTracking().Include(x => x.RolesPermisos).ThenInclude(x => x.Permiso).FirstOrDefaultAsync(m => m.IdRoles == id);
            if (rol == null)
            {
                return NotFound();
            }

            Input.IdRoles = rol.IdRoles;
            Input.Nombre = rol.Nombre;

            var permisos = await _context.Permisos.ToListAsync();

            if (permisos.Any())
            {
                foreach (var permiso in permisos)
                {
                    var permisoVM = new InputModel.PermisoVM() { IdPermisos = permiso.IdPermisos, Nombre = permiso.Nombre, Asignado = false };
                    if (rol.RolesPermisos.Find(x => x.IdPermisos == permiso.IdPermisos) != null)
                    {
                        permisoVM.Asignado = true;
                    }
                    Input.PermisosVM.Add(permisoVM);
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

            var rol = await _context.Roles.Include(x => x.RolesPermisos).FirstOrDefaultAsync(m => m.IdRoles == Input.IdRoles);
            if (rol == null)
            {
                return NotFound();
            }

            // Aunque ya debería estar trackeando al hacer la consulta anterior,
            // si no añades explícitamente ésto, el UserName no lo actualiza (aunque sea != del que ya tiene)
            // Sí actualiza los roles del usuario pero no el UserName...no entiendo
            _context.Attach(rol).State = EntityState.Modified;

            rol.Nombre = Input.Nombre;

            if (rol.RolesPermisos.Any())
            {
                _context.RolesPermisos.RemoveRange(rol.RolesPermisos);
            }

            var rolPermisos = new List<RolPermiso>();
            foreach (var permisoVM in Input.PermisosVM)
            {
                if (permisoVM.Asignado == true)
                {
                    rolPermisos.Add(new RolPermiso { IdRoles = Input.IdRoles, IdPermisos = permisoVM.IdPermisos });
                }
            }

            rol.RolesPermisos = rolPermisos;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(Input.IdRoles))
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
            return RedirectToPage("/Roles/Edit", new { id = rol.IdRoles });
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.IdRoles == id);
        }

    }
}
