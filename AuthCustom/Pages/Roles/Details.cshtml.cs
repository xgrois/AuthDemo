using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Roles
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.All, AuthCustom.Authorization.Definitions.Permisos.RolesPage.Read })]
    public class DetailsModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public DetailsModel(AuthCustom.Data.AuthCustomContext context)
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
    }
}
