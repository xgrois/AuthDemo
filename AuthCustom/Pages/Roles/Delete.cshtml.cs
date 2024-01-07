using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Roles
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.Delete })]
    public class DeleteModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public DeleteModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        #region BindProperties
        [BindProperty]
        public InputModel Input { get; set; } = new();
        #endregion

        #region ViewModels
        public class InputModel
        {
            public int IdRoles { get; set; }

            [Required]
            public string Nombre { get; set; } = string.Empty;
        }
        #endregion

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(m => m.IdRoles == id);

            if (rol == null)
            {
                return NotFound();
            }
            else
            {
                Input.IdRoles = rol.IdRoles;
                Input.Nombre = rol.Nombre;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FindAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
