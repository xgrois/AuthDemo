using AuthDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Pages.Usuarios
{
    public class DetailsModel : PageModel
    {
        private readonly AuthDemo.Data.AppDbContext _context;

        public DetailsModel(AuthDemo.Data.AppDbContext context)
        {
            _context = context;
        }

        public Usuario User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
