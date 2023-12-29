using AuthDemo.Authorization;
using AuthDemo.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Pages.Usuarios
{
    [Authorize(enuPermissions.ViewUsers)]
    public class IndexModel : PageModel
    {
        private readonly AuthDemo.Data.AppDbContext _context;

        public IndexModel(AuthDemo.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuarios { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Usuarios = await _context.Users.ToListAsync();
        }
    }
}
