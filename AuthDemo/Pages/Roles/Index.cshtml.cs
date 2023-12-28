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
    public class IndexModel : PageModel
    {
        private readonly AuthDemo.Data.AppDbContext _context;

        public IndexModel(AuthDemo.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Rol> Rol { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Rol = await _context.Roles.ToListAsync();
        }
    }
}
