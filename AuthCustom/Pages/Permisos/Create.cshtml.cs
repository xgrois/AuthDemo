using AuthCustom.Authorization.AuthorizeAttributes;
using AuthCustom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthCustom.Pages.Permisos
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.All, AuthCustom.Authorization.Definitions.Permisos.PermisosPage.Create })]
    public class CreateModel : PageModel
    {
        private readonly AuthCustom.Data.AuthCustomContext _context;

        public CreateModel(AuthCustom.Data.AuthCustomContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Permiso Permiso { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Permisos.Add(Permiso);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
