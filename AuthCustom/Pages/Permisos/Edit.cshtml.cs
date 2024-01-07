using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthCustom.Pages.Permisos
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.Update })]
    public class EditModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public EditModel(AuthCustom.Data.AuthCustomContext context)
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
            Permiso = permiso;
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

            _context.Attach(Permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(Permiso.IdPermisos))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.IdPermisos == id);
        }
    }
}
