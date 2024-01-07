using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthCustom.Pages.Roles
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.Read })]
    public class IndexModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public IndexModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public List<RolVM> RolesVM { get; set; } = new();

            public class RolVM
            {
                public int IdRoles { get; set; }
                public string Nombre { get; set; } = string.Empty;
            }
        }

        public async Task OnGetAsync()
        {
            var roles = await _context.Roles.ToListAsync();

            foreach (var rol in roles)
            {
                Input.RolesVM.Add(new InputModel.RolVM() { IdRoles = rol.IdRoles, Nombre = rol.Nombre });
            }
        }
    }
}
