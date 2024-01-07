using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Roles
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.Create })]
    public class CreateModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public CreateModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        #region InputModels
        public class InputModel
        {
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

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new();
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            var permisos = await _context.Permisos.ToListAsync();

            foreach (var permiso in permisos)
            {
                Input.PermisosVM.Add(new InputModel.PermisoVM() { IdPermisos = permiso.IdPermisos, Nombre = permiso.Nombre, Asignado = false });
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

            var rolYaExiste = await _context.Roles.FirstOrDefaultAsync(x => x.Nombre == Input.Nombre);
            if (rolYaExiste != null)
            {
                ModelState.AddModelError(string.Empty, "El rol ya existe.");
                return Page();
            }

            Rol rol = BuildRol(Input.Nombre, Input.PermisosVM);

            await _context.Roles.AddAsync(rol);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        #region Helpers
        private Rol BuildRol(string pNombre, List<InputModel.PermisoVM> pPermisosVM)
        {
            Rol rol = new Rol() { Nombre = pNombre };

            if (pPermisosVM != null && pPermisosVM.Count > 0)
            {
                rol.RolesPermisos = new List<RolPermiso>();
                foreach (var permisoVM in pPermisosVM)
                {
                    if (permisoVM.Asignado == true)
                    {
                        rol.RolesPermisos.Add(new RolPermiso()
                        {
                            IdRoles = rol.IdRoles,
                            IdPermisos = permisoVM.IdPermisos,
                        });
                    }
                }
            }

            return rol;
        }
        #endregion
    }
}
