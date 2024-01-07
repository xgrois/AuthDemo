﻿using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Usuarios
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.Read })]
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
            public int IdUsuarios { get; set; }

            [Required]
            [Display(Name = "Nombre de usuario")]
            public string UserName { get; set; } = string.Empty;
        }
        #endregion

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.IdUsuarios == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                Input.IdUsuarios = usuario.IdUsuarios;
                Input.UserName = usuario.UserName;
            }
            return Page();
        }
    }
}
