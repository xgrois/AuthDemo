using AuthCustom.Authorization.AuthorizeAttributes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthCustom.Pages.Usuarios
{
    [MyAuthorize(Permisos = new[] { AuthCustom.Authorization.Definitions.Permisos.Pages.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.All, AuthCustom.Authorization.Definitions.Permisos.UsuariosPage.Read })]
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
            public List<UsuarioVM> UsuariosVM { get; set; } = new();

            public class UsuarioVM
            {
                public int IdUsuarios { get; set; }

                [Display(Name = "Nombre de usuario")]
                public string UserName { get; set; } = string.Empty;

                [Display(Name = "Hash(Contraseña)")]
                public string HashedPassword { get; set; } = string.Empty;
            }
        }

        public async Task OnGetAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach (var usuario in usuarios)
            {
                Input.UsuariosVM.Add(new InputModel.UsuarioVM() { IdUsuarios = usuario.IdUsuarios, UserName = usuario.UserName, HashedPassword = usuario.Password });
            }
        }
    }
}
