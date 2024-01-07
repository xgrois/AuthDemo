using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthCustom.Pages.Permisos
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.Delete })]
    public class DeleteModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public DeleteModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Permiso Permiso { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FirstOrDefaultAsync(m => m.IdPermisos == id);

            if (permiso == null)
            {
                return NotFound();
            }
            else
            {
                Permiso = permiso;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso != null)
            {
                Permiso = permiso;
                _context.Permisos.Remove(Permiso);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
