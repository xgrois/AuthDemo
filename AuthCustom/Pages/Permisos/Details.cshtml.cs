using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthCustom.Pages.Permisos
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.Read })]
    public class DetailsModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public DetailsModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

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
    }
}
