using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthCustom.Pages.Permisos
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.Read })]
    public class IndexModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public IndexModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        public IList<Permiso> Permiso { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Permiso = await _context.Permisos.ToListAsync();
        }
    }
}
