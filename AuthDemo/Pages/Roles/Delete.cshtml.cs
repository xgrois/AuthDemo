using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AuthDemo.Data;
using AuthDemo.Models;

namespace AuthDemo.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly AuthDemo.Data.AppDbContext _context;

        public DeleteModel(AuthDemo.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rol Rol { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);

            if (rol == null)
            {
                return NotFound();
            }
            else
            {
                Rol = rol;
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
                Rol = rol;
                _context.Roles.Remove(Rol);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
